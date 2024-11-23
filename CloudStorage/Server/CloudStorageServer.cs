// MIT License
// Copyright (c) 2024 Marat

using CloudStorageLibrary;
using CloudStorageLibrary.Serializers;
using CloudStorageLibrary.Serializers.Exceptions;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    /// <summary>
    /// Implements the logic of communication between server and client
    /// </summary>
    internal class CloudStorageServer : IDisposable
    {
        private bool _disposed;
        
        /// <summary><see cref="SocketFacade"/> for sending responses and receiving requests</summary>
        private SocketFacade _mainSocket { get; set; }
        private SocketFacade _dataTransfer { get; set; }

        public CloudStorageServer(string host, int mainPort, int dataTransferPort)
        {
            Socket mainSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            mainSocket.Bind(new IPEndPoint(IPAddress.Parse(host), mainPort));
            mainSocket.Listen();

            Socket dataTransferSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            dataTransferSocket.Bind(new IPEndPoint(IPAddress.Parse(host), dataTransferPort));
            dataTransferSocket.Listen();

            _mainSocket = new SocketFacade(mainSocket);
            _dataTransfer = new SocketFacade(dataTransferSocket);
        }

        public CloudStorageClient AcceptClient()
        {
            Socket client = _mainSocket.Socket.Accept();
            Socket clientDataTrans = _dataTransfer.Socket.Accept();

            return new CloudStorageClient(new SocketFacade(client), new SocketFacade(clientDataTrans));
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
                    _mainSocket.Dispose();
                    _dataTransfer.Dispose();
                }

                _disposed = true;
            }
        }
    }
}
