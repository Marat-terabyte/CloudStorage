﻿// MIT License
// Copyright (c) 2024 Marat

using System.Net.Sockets;
using CloudStorageLibrary;
using CloudStorageLibrary.Serializers;
using CloudStorageLibrary.Serializers.Exceptions;

namespace ClientLibrary
{
    /// <summary>
    /// Implements the logic of communication between server and client
    /// </summary>
    public class CloudStorageClient : IDisposable
    {
        private bool _disposed;

        public SocketFacade MainSocket {  get; set; }
        public SocketFacade DataSocket { get; set; }

        public int ReceiveBufferSize { get; set; } = 1024;

        public CloudStorageClient(SocketFacade mainSocket, SocketFacade dataSocket)
        {
            MainSocket = mainSocket;
            DataSocket = dataSocket;
        }

        public CloudStorageClient(Socket mainSocket, Socket dataSocket)
        {
            MainSocket = new SocketFacade(mainSocket);
            DataSocket = new SocketFacade(dataSocket);
        }

        /// <summary> Receives a string response from the server and deserializes it </summary>
        /// <exception cref="SocketException"/>
        /// <exception cref="NotSupportedStatus"/>
        /// <exception cref="NotValidByteLength"/>
        public Response ReceiveResponse()
        {
            Response? response = null;
            while (response == null)
            {
                string data = MainSocket.Receive(256);
                response = ResponseSerializer.Deserialize(data);
            }

            return response;
        }

        /// <summary> Receives response and data from the server</summary>
        /// <param name="data">Data that receives from the server</param>
        /// <exception cref="SocketException"/>
        /// <exception cref="NotSupportedStatus"/>
        /// <exception cref="NotValidByteLength"/>
        public Response ReceiveResponse(out string? data)
        {
            Response response = ReceiveResponse();
            data = DataSocket.Receive((int) response.DataLenght);

            return response;
        }

        /// <summary> Serializes a request and sends it to the server </summary>
        /// <param name="request"> Client request to serialize and send </param>
        /// <exception cref="SocketException"/>
        public void SendRequest(Request request)
        {
            string serializedRequest = RequestSerializer.Serialize(request);

            MainSocket.Send(serializedRequest);
        }

        /// <summary> Disconnects from the server </summary>
        public void CloseConnection()
        {
            MainSocket.CloseConnection();
            DataSocket.CloseConnection();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    CloseConnection();
                    MainSocket.Dispose();
                    DataSocket.Dispose();
                }

                _disposed = true;
            }
        }
    }
}
