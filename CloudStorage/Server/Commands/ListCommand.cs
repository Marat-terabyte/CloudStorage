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

        public ListCommand(CloudStorageServer server, SocketFacade dataTransfer, string basePath) : base(server, dataTransfer)
        {
            _basePath = basePath;
        }

        public override void Execute(Request request)
        {
            string path;

            if (request.Args.Length > 0)
                path = _basePath + request.Args[0];
            else
                path = _basePath;

            if (!Directory.Exists(path))
            {
                Response response = new Response();
                response.Status = CommandStatus.NotOk;
                _server!.SendResponse(response);
            }
            else
            {
                var dirsAndFiles = Encoding.UTF8.GetBytes(GetFilesAndFolders(path));

                Response response = new Response();
                response.Status = CommandStatus.Ok;
                response.DataLenght = dirsAndFiles.Length;
                _server!.SendResponse(response);

                _dataTransfer!.SendBytes(dirsAndFiles);
            }
        }

        private string GetFilesAndFolders(string path)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine("dir: ..");

            foreach (var dir in Directory.GetDirectories(path))
            {
                stringBuilder.Append("dir: " + dir + '\n');
            }

            foreach (var file in Directory.GetFiles(path))
            {
                stringBuilder.Append("file: " + file + '\n');
            }

            return stringBuilder.ToString();
        }
    }
}
