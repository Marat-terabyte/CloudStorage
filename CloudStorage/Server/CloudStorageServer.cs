// MIT License
// Copyright (c) 2024 Marat

using CloudStorageLibrary;
using CloudStorageLibrary.Serializers;
using CloudStorageLibrary.Serializers.Exceptions;
using System.Net.Sockets;

namespace Server
{
    /// <summary>
    /// Implements the logic of communication between server and client
    /// </summary>
    internal class CloudStorageServer
    {
        private SocketFacade _socket;

        public int ReceiveBufferSize { get; set; } = 2048;

        public CloudStorageServer(Socket socket)
        {
            _socket = new SocketFacade(socket);
        }

        /// <summary> Receives a string request from the user and deserializes it </summary>
        /// <exception cref="SocketException"/>
        /// <exception cref="NotSupportedCommand"/>
        public Request ReceiveRequest()
        {
            string serializedRequest = _socket.Receive();

            return RequestSerializer.Deserialize(serializedRequest);
        }

        /// <summary> Serializes a response and sends it to the user </summary>
        /// <param name="response"> Server response to serialize and send </param>
        /// <exception cref="SocketException"/>
        public void SendResponse(Response response)
        {
            string serializedResponse = ResponseSerializer.Serialize(response);

            _socket.Send(serializedResponse);
        }

        /// <summary> Disconnects the user </summary>
        public void CloseConnection() => _socket.CloseConnection();
    }
}
