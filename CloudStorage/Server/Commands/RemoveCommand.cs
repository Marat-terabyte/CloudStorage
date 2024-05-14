// MIT License
// Copyright (c) Marat

using CloudStorageLibrary;
using CloudStorageLibrary.Commands;

namespace Server.Commands
{
    internal class RemoveCommand : RequestCommand
    {
        private string _basePath;

        public RemoveCommand(CloudStorageServer server, string basePath) : base(server)
        {
            _basePath = basePath;
        }

        protected override void DoAction(Request request)
        {
            foreach (var item in request.Args)
            {
                string path = Path.Combine(_basePath, item);
                if (File.Exists(path))
                    File.Delete(path);
                else if (Directory.Exists(path))
                    Directory.Delete(path);
            }

            Response response = new Response(CommandStatus.Ok);
            _server!.SendResponse(response, "Сompleted successfully");
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
