// MIT License
// Copyright (c) 2024 Marat

using Server.Commands;
using CloudStorageLibrary;
using CloudStorageLibrary.Commands;
using Server.Database;

namespace Server
{
    internal class CommandHandler
    {
        public long SessionID {  get; set; }
        public CloudStorageClient Client { get; set; }
        public string BasePath { get; set; }
        public IUserRepository Repository { get; set; }

        public CommandHandler(CloudStorageClient client, long sessionId, IUserRepository repository, string basePath)
        {
            SessionID = sessionId;
            Client = client;
            Repository = repository;
            BasePath = basePath;
        }

        public RequestCommand? GetCommand(Request request)
        {
            switch (request.Command)
            {
                case Command.List:
                    return new ListCommand(Client, BasePath);
                case Command.Download:
                    return new DownloadCommand(Client, BasePath);
                case Command.Remove:
                    return new RemoveCommand(Client, BasePath);
                case Command.Upload:
                    return new UploadCommand(Client, BasePath);
                case Command.SignIn:
                    return new SignInCommand(Client, SessionID, Repository);
                case Command.SignUp:
                    return new SignUpCommand(Client, Repository);
                default:
                    return null;
            }
        }
    }
}
