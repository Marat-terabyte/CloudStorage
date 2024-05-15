// MIT License
// Copyright (c) 2024 Marat

using System.Net;
using System.Net.Sockets;

namespace ClientLibrary.Commands
{
    public abstract class ClientCommand
    {
        public CloudStorageClient Client { get; private set; }

        /// <summary>Connects to the server and initializes <see cref="Client"/></summary>
        /// <exception cref="SocketException"></exception>
        public ClientCommand()
        {
            Socket mainSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Socket dataTransfer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            IPAddress address = IPAddress.Parse(ServerInfo.Host);
            mainSocket.Connect(new IPEndPoint(address, ServerInfo.MainPort));
            dataTransfer.Connect(new IPEndPoint(address, ServerInfo.DataTransferPort));

            Client = new CloudStorageClient(mainSocket, dataTransfer);
        }
    }
}
