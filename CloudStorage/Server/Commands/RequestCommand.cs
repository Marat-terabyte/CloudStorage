// MIT License
// Copyright (c) 2024 Marat

using CloudStorageLibrary;
using CloudStorageLibrary.Commands;

namespace Server.Commands
{
    internal abstract class RequestCommand
    {
        protected CloudStorageServer _server;

        public RequestCommand(CloudStorageServer server)
        {
            _server = server;
        }

        public void Execute(Request request)
        {
            if (CanExecute(request.Args, out string? errorMessage))
                DoAction(request);
            else
            {
                Response errorResponse = new Response(CommandStatus.NotOk);
                if (errorMessage != null)
                    _server.SendResponse(errorResponse, errorMessage);
                else
                    _server.SendResponse(errorResponse);
            }
        }
        
        protected abstract void DoAction(Request request);
        protected abstract bool CanExecute(object parameter, out string? errorMessage);

    }
}
