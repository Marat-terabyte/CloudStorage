// MIT License
// Copyright (c) 2024 Marat

using ClientLibrary.CloudElements;
using CloudStorageLibrary;
using CloudStorageLibrary.Commands;

namespace ClientLibrary.Commands
{
    public class DownloadCommand : ClientCommand
    {
        private FileTransfer _fileTransfer;

        public CloudElement CloudElement { get; set; }
        public string ToDir { get; set; } = "";

        public DownloadCommand(CloudElement elementToDownload)
        {
            _fileTransfer = new FileTransfer(Client.DataSocket);
            CloudElement = elementToDownload;
        }

        public DownloadCommand(CloudElement elementToDownload, string toDir)
        {
            _fileTransfer = new FileTransfer(Client.DataSocket);
            CloudElement = elementToDownload;
            ToDir = toDir;
        }

        public bool Execute(out string? message)
        {
            Request request = RequestBuilder.Build(Command.Download, [CloudElement.Path]);
            Client.SendRequest(request);
            
            Response response = Client.ReceiveResponse();
            if (response.Status == CommandStatus.NotOk)
            {
                message = Client.DataSocket.Receive((int)response.DataLenght);

                return false;
            }

            _fileTransfer.ReceiveFile(Path.Combine(ToDir, CloudElement.Name), response.DataLenght);
            message = $"{CloudElement.Name} downloaded successfully";

            return true;
        }
    }
}
