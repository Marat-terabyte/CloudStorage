// MIT License
// Copyright (c) 2024 Marat

using System.Net.Sockets;
using CloudStorageLibrary;
using CloudStorageLibrary.Serializers;
using CloudStorageLibrary.Serializers.Exceptions;

namespace Client
{
    /// <summary>
    /// Implements the logic of communication between server and client
    /// </summary>
    internal class CloudStorageClient
    {
        private SocketFacade _socket;

        public int ReceiveBufferSize { get; set; } = 1024;

        public CloudStorageClient(SocketFacade socket)
        {
            _socket = socket;
        }

        /// <summary> Receives a string response from the server and deserializes it </summary>
        /// <exception cref="SocketException"/>
        /// <exception cref="NotSupportedStatus"/>
        /// <exception cref="NotValidByteLength"/>
        public Response ReceiveResponse()
        {
            string serializedResponse = _socket.Receive(ReceiveBufferSize);

            return ResponseSerializer.Deserialize(serializedResponse);
        }

        /// <summary> Serializes a request and sends it to the server </summary>
        /// <param name="request"> Client request to serialize and send </param>
        /// <exception cref="SocketException"/>
        public void SendRequest(Request request)
        {
            string serializedRequest = RequestSerializer.Serialize(request);

            _socket.Send(serializedRequest);
        }

        /// <summary> Disconnects from the server </summary>
        public void CloseConnection() => _socket.CloseConnection();
    }
}
