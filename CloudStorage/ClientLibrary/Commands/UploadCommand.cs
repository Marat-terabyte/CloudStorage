// MIT License
// Copyright (c) 2024 Marat

using ClientLibrary.CloudElements;
using CloudStorageLibrary;
using CloudStorageLibrary.Commands;

namespace ClientLibrary.Commands
{
    public class UploadCommand : ClientCommand
    {
        private FileTransfer _fileTransfer;

        public string Filename { get; set; }
        public string FromCloudDir { get; set; } = "";

        public UploadCommand(string path)
        {
            _fileTransfer = new FileTransfer(Client.DataSocket);
            Filename = path;

        }
        public UploadCommand(string path, string fromCloudDir)
        {
            _fileTransfer = new FileTransfer(Client.DataSocket);
            Filename = path;
            FromCloudDir = fromCloudDir;
        }

        public bool Execute(out string? message)
        {
            if (!File.Exists(Filename) && !Directory.Exists(Filename))
            {
                message = $"{Filename} does not exist";

                return false;
            }

            FileAttributes attr = File.GetAttributes(Filename);
            if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
            {
                string[] files = Directory.GetFiles(Filename);
                string[] dirs = Directory.GetDirectories(Filename);
                
                foreach (string file in files)
                {
                    new UploadCommand(file, Path.Combine(FromCloudDir, new DirectoryInfo(Filename).Name)).Execute(out message);
                }

                foreach (string dir in dirs)
                    new UploadCommand(dir, Path.Combine(FromCloudDir, new DirectoryInfo(Filename).Name)).Execute(out message);
            }
            else
            {
                SendRequest();

                Response response = Client.ReceiveResponse();
                if (response.Status == CommandStatus.NotOk)
                {
                    message = Client.DataSocket.Receive((int)response.DataLenght);

                    return false;
                }

                _fileTransfer.SendFile(Filename);
            }

            message = $"{Filename} uploaded successfully";

            return true;
        }

        private void SendRequest()
        {
            var file = new FileInfo(Filename);
            Request request = RequestBuilder.Build(Command.Upload, [Path.Combine(FromCloudDir, file.Name), file.Length.ToString()]);
            Client.SendRequest(request);
        }
    }
}
