// MIT License
// Copyright (c) 2024 Marat

using CloudStorageLibrary;
using CloudStorageLibrary.Commands;

namespace Server.Commands
{
    internal class UploadCommand : RequestCommand
    {
        private string _basePath;
        private FileTransfer _fileTransfer;

        public UploadCommand(CloudStorageClient client, string basePath) : base(client)
        {
            _basePath = basePath;
            _fileTransfer = new FileTransfer(client.DataTransfer);
        }

        protected override bool CanExecute(Request request, out string? errorMessage)
        {
            if (request == null || request.Args.Length < 2)
            {
                errorMessage = "Not correct request";

                return false;
            }
            errorMessage = null;
            
            // The first argument is filename
            // The second argument is size of a file

            bool isSize = long.TryParse(request.Args[1], out long size);
            if (!isSize)
            {
                errorMessage = "File size not received";

                return false;
            }

            return true;
        }

        protected override void DoAction(Request request)
        {
            string filename = request.Args[0];
            long size = long.Parse(request.Args[1]);

            Response response = new Response(CommandStatus.Ok);
            _client.SendResponse(response);
            
            _fileTransfer.ReceiveFile(Path.Combine(_basePath, filename), size);
        }
    }
}
