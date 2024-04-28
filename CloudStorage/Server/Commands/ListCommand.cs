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

        public override void Execute()
        {
            var dirsAndFiles = Encoding.UTF8.GetBytes(GetFilesAndFolders());

            Response response = new Response();
            response.Status = CommandStatus.Ok;
            response.DataLenght = dirsAndFiles.Length;
            _server!.SendResponse(response);

            _dataTransfer!.SendBytes(dirsAndFiles);
        }

        private string GetFilesAndFolders()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine("dir: ..");

            if (Directory.Exists(_basePath))
            {
                foreach (var dir in Directory.GetDirectories(_basePath))
                {
                    stringBuilder.Append("dir: " + dir + '\n');
                }

                foreach (var file in Directory.GetFiles(_basePath))
                {
                    stringBuilder.Append("file: " +  file + '\n');
                }
            }

            return stringBuilder.ToString();
        }
    }
}
