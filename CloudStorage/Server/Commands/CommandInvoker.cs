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

        public CommandInvoker(CloudStorageServer server, SocketFacade dataTransfer, string basePath)
        {
            _server = server;
            _socketFacade = dataTransfer;

            _listCommand = new ListCommand(server, dataTransfer, basePath);
        }

        public void Invoke(Request request)
        {
            switch (request.Command)
            {
                case Command.List:
                    _listCommand.Execute();
                    break;
                case Command.Download:
                    break;
                case Command.Upload:
                    break;
                case Command.SignIn:
                    break;
                case Command.SignOut:
                    break;
                case Command.Remove:
                    break;
            }
        }
    }
}
