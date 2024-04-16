using System.Text;
using System.Net.Sockets;

namespace CloudStorageLibrary
{
    public class SocketFacade
    {
        public Socket Socket { get; set; }

        public bool IsConnected { get => Socket.Connected; }

        public SocketFacade(Socket socket)
        {
            Socket = socket;
        }

        public string Receive()
        {
            return Receive(Socket.ReceiveBufferSize);
        }

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

        public void CloseConnection()
        {
            if (IsConnected)
                Socket.Shutdown(SocketShutdown.Both);
            
            Socket.Close();
        }
    }
}
