using CloudStorageLibrary;
using CloudStorageLibrary.Serializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    internal class CloudStorageClient
    {
        private bool _disposed;

        public SocketFacade MainSocket { get; }
        public SocketFacade DataTransfer { get; }

        public CloudStorageClient(SocketFacade mainSocket, SocketFacade dataTransfer)
        {
            MainSocket = mainSocket;
            DataTransfer = dataTransfer;
        }

        /// <summary> Receives a string request from the user and deserializes it </summary>
        /// <exception cref="SocketException"/>
        /// <exception cref="NotSupportedCommand"/>
        public Request ReceiveRequest()
        {
            Request? request = null;
            while (request == null)
            {
                string data = MainSocket.Receive(1024);
                request = RequestSerializer.Deserialize(data);
            }

            return request;
        }

        /// <summary> Serializes a response and sends it to the user </summary>
        /// <param name="response"> Server response to serialize and send </param>
        /// <exception cref="SocketException"/>
        public void SendResponse(Response response)
        {
            string serializedResponse = ResponseSerializer.Serialize(response);

            MainSocket.Send(serializedResponse);
        }

        /// <summary>
        /// Serializes <paramref name="response"/>, encodes <paramref name="message"/> and sends them to the user
        /// </summary>
        public void SendResponse(Response response, string message)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            response.DataLenght = buffer.Length;

            SendResponse(response);
            DataTransfer.SendBytes(buffer);
        }

        /// <summary>
        /// Serializes <paramref name="response"/>, sends it and <paramref name="message"/>
        /// </summary>
        public void SendResponse(Response response, byte[] message)
        {
            response.DataLenght = message.Length;

            SendResponse(response);
            DataTransfer.SendBytes(message);
        }

        /// <summary> Disconnects the user </summary>
        public void CloseConnection()
        {
            MainSocket.CloseConnection();
            DataTransfer.CloseConnection();
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
                    MainSocket.Dispose();
                    DataTransfer.Dispose();
                }

                _disposed = true;
            }
        }
    }
}
