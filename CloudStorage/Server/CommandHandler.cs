﻿using Server.Commands;
using CloudStorageLibrary;
using CloudStorageLibrary.Commands;
using System.Net;

namespace Server
{
    internal class CommandHandler
    {
        public CloudStorageServer Server { get; set; }
        public SocketFacade DataTransfer { get; set; }
        public string BasePath { get; set; }

        public CommandHandler(CloudStorageServer server, SocketFacade dataTransfer, string basePath)
        {
            Server = server;
            DataTransfer = dataTransfer;
            BasePath = basePath;
        }

        public RequestCommand? GetCommand(Request request)
        {
            switch (request.Command)
            {
                case Command.List:
                    return new ListCommand(Server, DataTransfer, BasePath);
                case Command.Download:
                    return new DownloadCommand(Server, DataTransfer, BasePath);
                case Command.Remove:
                    return new RemoveCommand(Server, DataTransfer, BasePath);
                default:
                    return null;
            }
        }
    }
}