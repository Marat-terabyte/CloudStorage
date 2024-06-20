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

        public string Path { get; set; }
        public string ToCloudDir { get; set; } = "";

        public UploadCommand(string path)
        {
            _fileTransfer = new FileTransfer(Client.DataSocket);
            Path = path;

        }
        public UploadCommand(string path, string fromCloudDir)
        {
            _fileTransfer = new FileTransfer(Client.DataSocket);
            Path = path;
            ToCloudDir = fromCloudDir;
        }

        public bool Execute(out string? message)
        {
            if (!File.Exists(Path) && !Directory.Exists(Path))
            {
                message = $"{Path} does not exist";

                return false;
            }

            FileAttributes attr = File.GetAttributes(Path);
            if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
            {
                string[] files = Directory.GetFiles(Path);
                string[] dirs = Directory.GetDirectories(Path);
                
                foreach (string file in files)
                {
                    new UploadCommand(file, System.IO.Path.Combine(ToCloudDir, new DirectoryInfo(Path).Name)).Execute(out message);
                }

                foreach (string dir in dirs)
                    new UploadCommand(dir, System.IO.Path.Combine(ToCloudDir, new DirectoryInfo(Path).Name)).Execute(out message);
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

                _fileTransfer.SendFile(Path);
            }

            message = $"{Path} uploaded successfully";

            return true;
        }

        private void SendRequest()
        {
            var file = new FileInfo(Path);
            Request request = RequestBuilder.Build(Command.Upload, [System.IO.Path.Combine(ToCloudDir, file.Name), file.Length.ToString()]);
            Client.SendRequest(request);
        }
    }
}
