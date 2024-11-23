// MIT License
// Copyright (c) Marat

using CloudStorageLibrary;
using CloudStorageLibrary.Commands;
using System.IO;

namespace Server.Commands
{
    internal class RemoveCommand : RequestCommand
    {
        private string _basePath;

        public RemoveCommand(CloudStorageClient client, string basePath) : base(client)
        {
            _basePath = basePath;
        }

        protected override void DoAction(Request request)
        {
            Response response;
            string? path = null;
            try
            {
                DeleteFiles(request.Args, out path);

                response = new Response(CommandStatus.Ok);
                _client!.SendResponse(response, "Сompleted successfully");
            }
            catch (Exception)
            {
                response = new Response(CommandStatus.NotOk);
                _client!.SendResponse(response, $"{path} could not be deleted. Try again later");
            }
        }

        /// <param name="path">is the path of the file that could not be deleted</param>
        private void DeleteFiles(string[] files, out string? path)
        { 
            foreach (var item in files)
            {
                path = item;
                string fullPath = Path.Combine(_basePath, item);
                if (File.Exists(fullPath))
                    File.Delete(fullPath);
                else if (Directory.Exists(fullPath))
                    Directory.Delete(fullPath, true);
            }

            path = null;
        }

        protected override bool CanExecute(Request request, out string? errorMessage)
        {
            errorMessage = null;

            if (request == null || request.Args.Length == 0)
            {
                errorMessage = "Not correct request";

                return false;
            }

            return true;
        }
    }
}
