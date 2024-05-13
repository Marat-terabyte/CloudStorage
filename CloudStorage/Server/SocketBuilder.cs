// MIT License
// Copyright (c) 2024 Marat

using System.Net;
using System.Net.Sockets;

namespace Server
{
    internal static class SocketBuilder
    {
        public static Socket Build(string ipAddress, int port)
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(new IPEndPoint(IPAddress.Parse(ipAddress), port));
            socket.Listen();

            return socket;
        }
    }
}
