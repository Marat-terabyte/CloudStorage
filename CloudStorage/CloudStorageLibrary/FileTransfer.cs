// MIT License
// Copyright (c) 2024 Marat

using System.IO;
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

        public int BufferSize { get; set; } = 65536;

        public FileTransfer(Socket socket)
        {
            _socketFacade = new SocketFacade(socket);
        }

        public FileTransfer(SocketFacade socketFacade)
        {
            _socketFacade = socketFacade;
        }

        /// <summary>
        /// Sends the file <paramref name="filename"/> to the connected socket
        /// </summary>
        /// <param name="filename">The string contains path to the file to send</param>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="SocketException"></exception>
        public void SendFile(string filename)
        {
            using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                while (fs.Length - fs.Position > 0)
                {
                    _socketFacade.SendBytes(ReadFile(fs, BufferSize));
                }
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
        /// <param name="filename">The path to create of the received file</param>
        /// <param name="sizeOfFile">The file size that will be received</param>
        /// <exception cref="SocketException"></exception>
        public void ReceiveFile(string filename, long sizeOfFile)
        {
            CreateDictionary(filename, out string? dirName);
            CreateNewFile(filename, dirName, out filename);

            using FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write);
            int len = 0;
            do
            {
                byte[] bytes = _socketFacade.ReceiveBytes(BufferSize, out len);
                fs.Write(bytes, 0, len);
            }
            while (len != 0);
        }

        private void CreateNewFile(string filename, string? dirName, out string newFilename)
        {
            int count = 0;
            FileInfo fileInfo = new FileInfo(filename);
            while (IsFileLockedByAnotherProcess(filename))
            {
                string tempFilename = $"{Path.GetFileNameWithoutExtension(fileInfo.Name)}({count++}){fileInfo.Extension}";
                if (dirName == null)
                    filename = tempFilename;
                else
                    filename = Path.Combine(dirName, tempFilename);
            }

            newFilename = filename;
        }

        private bool IsFileLockedByAnotherProcess(string filename)
        {
            try
            {
                using FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write);
                fs.Close();

                return false;
            }
            catch (IOException)
            {
                return true;
            }
        }

        private void CreateDictionary(string filename, out string? dirName)
        {
            dirName = filename.Substring(0, filename.LastIndexOf('\\') + 1);
            if (dirName != null && !string.IsNullOrWhiteSpace(dirName))
                Directory.CreateDirectory(dirName);
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
