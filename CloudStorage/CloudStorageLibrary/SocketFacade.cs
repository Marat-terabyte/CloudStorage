// MIT License
// Copyright (c) 2024 Marat

using System.Text;
using System.Net.Sockets;

namespace CloudStorageLibrary
{
    /// <summary>
    /// Provides socket functions
    /// </summary>
    public class SocketFacade : IDisposable
    {
        private bool _disposedSocket;

        public Socket Socket { get; set; }
        public bool IsConnected { get => Socket?.Connected ?? false; }

        public SocketFacade(Socket socket)
        {
            Socket = socket;
        }

        /// <summary> Receives bytes with the size of the array containing them 8192 bytes and decodes them </summary>
        /// <returns> Received data from the connected socket </returns>
        /// <exception cref="SocketException"></exception>
        public string Receive()
        {
            return Receive(Socket.ReceiveBufferSize);
        }

        /// <summary> Receives bytes and decodes them </summary>
        /// <param name="bufferSize"> The size of array contains received bytes </param>
        /// <returns> Received data from the connected socket </returns>
        /// <exception cref="SocketException"></exception>
        public string Receive(int bufferSize)
        {
            if (!IsConnected)
            {
                Socket.Close();
                throw new SocketException((int) SocketError.NotConnected, $"Socket is disconnected. Error code: {SocketError.NotConnected}");
            }
            
            byte[] dataBytes = new byte[bufferSize];
            
            int numberOfBytes = Socket.Receive(dataBytes);
            string data = Encoding.UTF8.GetString(dataBytes, 0, numberOfBytes);

            return data;
        }

        /// <summary> Encodes data and sends it to the connected socket </summary>
        /// <param name="data"> The value to send </param>
        /// <exception cref="SocketException"/>
        public void Send(string data)
        {
            if (!IsConnected)
            {
                Socket.Close();
                throw new SocketException((int)SocketError.NotConnected, $"Socket is disconnected. Error code: {SocketError.NotConnected}");
            }

            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            Socket.Send(dataBytes);
        }

        /// <summary> Closes the socket connection </summary>
        public void CloseConnection()
        {
            if (IsConnected)
                Socket.Shutdown(SocketShutdown.Both);
            
            Socket.Close();
        }

        // Implementation of Dispose pattern
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedSocket)
            {
                if (disposing)
                {
                    CloseConnection();
                    Socket.Dispose();
                }

                _disposedSocket = true;
            }
        }
    }
}
