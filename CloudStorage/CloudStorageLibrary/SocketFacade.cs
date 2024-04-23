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
        public bool IsConnected
        {
            get =>
                !(Socket.Available == 0 && Socket.Poll(1000, SelectMode.SelectRead));
        }

        public SocketFacade(Socket socket)
        {
            if (!socket.Connected)
                throw new SocketException((int)SocketError.NotConnected, $"Socket is disconnected. Error code: {SocketError.NotConnected}");

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
            byte[] dataBytes = ReceiveBytes(bufferSize, out int numberOfBytes);
            string data = Encoding.UTF8.GetString(dataBytes, 0, numberOfBytes);

            return data;
        }

        /// <summary>
        /// Receives bytes with the size of array containing them 8192 bytes
        /// </summary>
        /// <exception cref="SocketException"></exception>
        public byte[] ReceiveBytes()
        {
            return ReceiveBytes(Socket.ReceiveBufferSize);
        }

        /// <summary>
        /// Receives bytes from the connected socket
        /// </summary>
        /// <param name="bufferSize">The size of array contains received bytes</param>
        /// <exception cref="SocketException"></exception>
        public byte[] ReceiveBytes(int bufferSize)
        {
            byte[] dataBytes = new byte[bufferSize];
            Socket.Receive(dataBytes);

            return dataBytes;
        }

        /// <summary>
        /// Receives bytes from the connected socket
        /// </summary>
        /// <param name="bufferSize">The size of array contains received bytes</param>
        /// <param name="numberOfBytes">The number of bytes received</param>
        /// <exception cref="SocketException"></exception>
        public byte[] ReceiveBytes(int bufferSize, out int numberOfBytes)
        {
            byte[] dataBytes = new byte[bufferSize];
            numberOfBytes = Socket.Receive(dataBytes);

            return dataBytes;
        }

        /// <summary>
        /// Receives bytes with the size of the array containing them 8192 bytes from the connected socket and converts them into <see cref="int"/>
        /// </summary>
        /// <exception cref="SocketException"></exception>
        public int ReceiveInt()
        {
            byte[] dataBytes = ReceiveBytes();

            return BitConverter.ToInt32(dataBytes);
        }

        /// <summary>
        /// Receives bytes from the connected socket and converts them into <see cref="int"/>
        /// </summary>
        /// <exception cref="SocketException"></exception>
        public int ReceiveInt(int bufferSize)
        {
            byte[] dataBytes = ReceiveBytes(bufferSize);

            return BitConverter.ToInt32(dataBytes);
        }

        /// <summary> Encodes data and sends it to the connected socket </summary>
        /// <param name="data"> The value to send </param>
        /// <exception cref="SocketException"/>
        public void Send(string data)
        {
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            SendBytes(dataBytes);
        }

        /// <summary>
        /// Converts <see cref="int"/> into <see cref="byte"/>[] and sends them
        /// </summary>
        /// <param name="value">The value to send</param>
        /// <exception cref="SocketError"></exception>
        public void Send(int value) => Socket.Send(BitConverter.GetBytes(value));

        /// <summary>
        /// Sends bytes to the connected socket
        /// </summary>
        /// <param name="bytes">The bytes to send</param>
        /// <exception cref="SocketException"></exception>
        public void SendBytes(byte[] bytes) => Socket.Send(bytes);

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
