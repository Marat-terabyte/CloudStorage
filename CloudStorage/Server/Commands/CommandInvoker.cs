// MIT License
// Copyright (c) 2024 Marat

using CloudStorageLibrary;
using CloudStorageLibrary.Commands;

namespace Server.Commands
{
    internal class CommandInvoker
    {
        private CloudStorageServer _server;
        private SocketFacade _socketFacade;

        private ListCommand _listCommand;
        private RemoveCommand _removeCommand;

        public CommandInvoker(CloudStorageServer server, SocketFacade dataTransfer, string basePath)
        {
            _server = server;
            _socketFacade = dataTransfer;

            _listCommand = new ListCommand(server, dataTransfer, basePath);
            _removeCommand = new RemoveCommand(server, dataTransfer, basePath);
        }

        public void Invoke(Request request)
        {
            switch (request.Command)
            {
                case Command.List:
                    _listCommand.Execute(request);
                    break;
                case Command.Remove:
                    _removeCommand.Execute(request);
                    break;
                case Command.Download:
                    break;
                case Command.Upload:
                    break;
                case Command.SignIn:
                    break;
                case Command.SignOut:
                    break;
            }
        }
    }
}
