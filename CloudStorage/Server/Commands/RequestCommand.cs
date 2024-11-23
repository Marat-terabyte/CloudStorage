// MIT License
// Copyright (c) 2024 Marat

using CloudStorageLibrary;
using CloudStorageLibrary.Commands;

namespace Server.Commands
{
    internal abstract class RequestCommand
    {
        protected CloudStorageClient _client;

        public RequestCommand(CloudStorageClient server)
        {
            _client = server;
        }

        public void Execute(Request request)
        {
            if (CanExecute(request, out string? errorMessage))
                DoAction(request);
            else
            {
                Response errorResponse = new Response(CommandStatus.NotOk);
                if (errorMessage != null)
                    _client.SendResponse(errorResponse, errorMessage);
                else
                    _client.SendResponse(errorResponse);
            }
        }
        
        protected abstract void DoAction(Request request);
        protected abstract bool CanExecute(Request request, out string? errorMessage);

    }
}
