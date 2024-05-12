// MIT License
// Copyright (c) 2024 Marat

using Server.Commands;
using CloudStorageLibrary;
using CloudStorageLibrary.Commands;
using Server.Database;
using Server.Database.Models;

namespace Server
{
    internal class CommandHandler
    {
        public CloudStorageServer Server { get; set; }
        public string BasePath { get; set; }
        public IUserRepository Repository { get; set; }

        public CommandHandler(CloudStorageServer server, IUserRepository repository, string basePath)
        {
            Server = server;
            Repository = repository;
            BasePath = basePath;
        }

        public RequestCommand? GetCommand(Request request)
        {
            switch (request.Command)
            {
                case Command.List:
                    return new ListCommand(Server, BasePath);
                case Command.Download:
                    return new DownloadCommand(Server, BasePath);
                case Command.Remove:
                    return new RemoveCommand(Server, BasePath);
                case Command.Upload:
                    return new UploadCommand(Server, BasePath);
                case Command.SignIn:
                    return new SignInCommand(Server, Repository);
                case Command.SignUp:
                    return new SignUpCommand(Server, Repository);
                default:
                    return null;
            }
        }
    }
}
