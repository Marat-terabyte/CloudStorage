// MIT License
// Copyright (c) Marat

using CloudStorageLibrary;
using CloudStorageLibrary.Commands;

namespace Server.Commands
{
    internal class RemoveCommand : RequestCommand
    {
        private string _basePath;

        public RemoveCommand(CloudStorageServer server, SocketFacade dataTransfer, string basePath) : base(server, dataTransfer)
        {
            _basePath = basePath;
        }

        public override void Execute(Request request)
        {
            Response response;

            if (request.Args.Length == 0)
            {
                response = new Response();
                response.Status = CommandStatus.NotOk;
                _server!.SendResponse(response);

                return;
            }

            foreach (var item in request.Args)
            {
                if (File.Exists(item))
                    File.Delete(item);
                else if (Directory.Exists(item))
                    Directory.Delete(item);
            }

            response = new Response();
            response.Status = CommandStatus.Ok;
            _server!.SendResponse(response);
        }
    }
}
