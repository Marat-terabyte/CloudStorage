// MIT License
// Copyright (c) 2024 Marat

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Server
{
    public class SocketBuilder
    {
        public Socket BuildSocket(string host, int port)
        {
            Socket socket = CreateAndBind(host, port);
            socket.Listen();

            return socket;
        }

        public Socket BuildSocket(string host, int port, int lengthOfConnectionsQueue)
        {
            Socket socket = CreateAndBind(host, port);
            socket.Listen(lengthOfConnectionsQueue);

            return socket;
        }

        private Socket CreateAndBind(string host, int port)
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(new IPEndPoint(IPAddress.Parse(host), 7000));

            return socket;
        }
    }
}
