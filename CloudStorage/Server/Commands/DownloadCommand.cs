// MIT License
// Copyright (c) 2024 Marat

using CloudStorageLibrary;
using CloudStorageLibrary.Commands;

namespace Server.Commands
{
    internal class DownloadCommand : RequestCommand
    {
        private string _basePath;
        private FileTransfer _fileTransfer;

        public DownloadCommand(CloudStorageServer server, string basePath) : base(server)
        {
            _basePath = basePath;
            _fileTransfer = new FileTransfer(server.DataTransfer);
        }

        protected override void DoAction(Request request)
        {
            string file = _basePath + request.Args[0];

            Response response = new Response(CommandStatus.Ok, new FileInfo(file).Length);
            _server!.SendResponse(response);
            _fileTransfer.SendFile(file);

        }

        protected override bool CanExecute(Request request, out string? errorMessage)
        {
            errorMessage = null;

            if (request == null)
            {
                errorMessage = "Not correct request";

                return false;
            }

            string[]? paths = request.Args;

            string[] notExistFiles = CheckFiles(paths);
            if (notExistFiles.Length != 0)
            {
                if (notExistFiles.Length == 1)
                    errorMessage = $"{notExistFiles[0]} does not exist";
                else
                    errorMessage = string.Join(", ", notExistFiles) + " do not exist";

                return false;
            }

            return true;
        }

        /// <summary>
        /// Checks if files exist in the <paramref name="filenames"/> array
        /// </summary>
        /// <returns>Not exist files</returns>
        private string[] CheckFiles(string[] filenames)
        {
            List<string> notExistFiles = new List<string>();

            foreach (string filename in filenames)
            {
                if (!File.Exists(_basePath + filename))
                {
                    notExistFiles.Add(_basePath + filename);
                }
            }

            return notExistFiles.ToArray();
        }
    }
}
