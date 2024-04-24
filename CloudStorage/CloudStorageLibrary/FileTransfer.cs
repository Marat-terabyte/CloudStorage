// MIT License
// Copyright (c) 2024 Marat

using System;
using System.Net.Sockets;

namespace CloudStorageLibrary
{
    /// <summary>
    /// Provides functions for sending/receiving files
    /// </summary>
    public class FileTransfer : IDisposable
    {
        private SocketFacade _socketFacade;
        private bool _disposed;

        public int BufferSize { get; set; } = 4096;

        public FileTransfer(Socket socket)
        {
            _socketFacade = new SocketFacade(socket);
        }

        public FileTransfer(SocketFacade socketFacade)
        {
            _socketFacade = socketFacade;
        }

        /// <summary>
        /// Sends the file <paramref name="fileName"/> to the connected socket
        /// </summary>
        /// <param name="fileName">The string contains path to the file to send</param>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="SocketException"></exception>
        public void SendFile(string fileName)
        {
            // Receives the buffer size of the connected socket
            int size = _socketFacade.ReceiveInt();

            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                _socketFacade.Send(fs.Length);

                while (fs.Length - fs.Position > size)
                {
                    _socketFacade.SendBytes(ReadFile(fs, size));
                }

                _socketFacade.Send((int)(fs.Length - fs.Position)); // Sends the remaining file size
                _socketFacade.SendBytes(ReadFile(fs, size));
            }
        }

        private byte[] ReadFile(FileStream fs, int bufferSize)
        {
            byte[] buffer = new byte[bufferSize];
            fs.Read(buffer, 0, buffer.Length);

            return buffer;
        }

        /// <summary>
        /// Receives the file from the connected socket
        /// </summary>
        /// <param name="fileName">The path to create of the received file</param>
        /// <param name="sizeOfFile">The file size that will be received</param>
        /// <exception cref="SocketException"></exception>
        public void ReceiveFile(string fileName)
        {
            // Sends the buffer size to the connected socket
            _socketFacade.Send(BufferSize);

            long sizeOfFile = _socketFacade.ReceiveLong();

            long rounds = (long) Math.Ceiling(sizeOfFile / (float) BufferSize);

            using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                for (; rounds > 1; rounds--)
                {
                    byte[] buffer = _socketFacade.ReceiveBytes(BufferSize);
                    fs.Write(buffer, 0, buffer.Length);
                }

                // Receives the remaining file size
                int size = _socketFacade.ReceiveInt();

                byte[] buff = _socketFacade.ReceiveBytes(size);
                fs.Write(buff, 0, buff.Length);
            }
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
                    _socketFacade.Dispose();
                }

                _disposed = true;
            }
        }
    }
}
