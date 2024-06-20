// MIT License
// Copyright (c) 2024 Marat

using CloudStorageLibrary;
using CloudStorageLibrary.Commands;
using System.Text;

namespace Server.Commands
{
    internal class ListCommand : RequestCommand
    {
        private string _basePath;

        public ListCommand(CloudStorageServer server, string basePath) : base(server)
        {
            if (basePath[basePath.Length - 1] == '\\')
                _basePath = basePath;
            else
                _basePath = basePath + '\\';
        }

        protected override void DoAction(Request request)
        {
            string path;
            if (request.Args.Length == 0)
                path = _basePath;
            else
                path = Path.Combine(_basePath, request.Args[0]);

            var dirsAndFiles = Encoding.UTF8.GetBytes(GetFilesAndFolders(path));
            
            Response response = new Response(CommandStatus.Ok, dirsAndFiles.Length);
            _server!.SendResponse(response, dirsAndFiles);
        }

        protected override bool CanExecute(Request request, out string? errorMessage)
        {
            errorMessage = null;

            if (request == null)
            {
                errorMessage = "Not correct request";

                return false;
            }

            string[]? args = request.Args;

            string path;
            if (args.Length == 0)
                path = _basePath;
            else
                path = Path.Combine(_basePath, args[0]);

            if (!Directory.Exists(path))
            {
                errorMessage = $"{path} does not exist";

                return false;
            }

            return true;
        }

        private string GetFilesAndFolders(string path)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine("dir: ..");

            foreach (var dir in Directory.GetDirectories(path))
            {
                string relativePath = dir.Replace(_basePath, "");
                stringBuilder.Append("dir: " + relativePath + '\n');
            }

            foreach (var file in Directory.GetFiles(path))
            {
                string relativePath = file.Replace(_basePath, "");
                stringBuilder.Append("file: " + relativePath + '\n');
            }

            return stringBuilder.ToString();
        }
    }
}
