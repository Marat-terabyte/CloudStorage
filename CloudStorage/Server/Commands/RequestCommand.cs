// MIT License
// Copyright (c) 2024 Marat

using CloudStorageLibrary;

namespace Server.Commands
{
    internal abstract class RequestCommand
    {
        protected CloudStorageServer? _server;
        protected SocketFacade? _dataTransfer;

        public RequestCommand(CloudStorageServer server ,SocketFacade? dataTransfer)
        {
            _server = server;
            _dataTransfer = dataTransfer;
        }

        public RequestCommand(SocketFacade? dataTransfer)
        {
             _dataTransfer = dataTransfer;
        }

        public abstract void Execute();
    }
}
