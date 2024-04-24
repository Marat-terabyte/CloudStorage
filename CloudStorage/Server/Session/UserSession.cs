// MIT License
// Copyright (c) 2024 Marat

using System.Net.Sockets;

namespace Server.Session
{
    internal class UserSession
    {
        private CloudStorageServer _server;

        public UserSession(Socket commandSocket, Socket dataTransferSocket)
        {
            _server = new CloudStorageServer(commandSocket);
        }

        public void Start()
        {
            while (true)
            {
                _server.ReceiveRequest();
            }
        }
    }
}
