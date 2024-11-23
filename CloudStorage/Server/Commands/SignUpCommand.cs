// MIT License
// Copyright (c) 2024 Marat

using CloudStorageLibrary;
using CloudStorageLibrary.Commands;
using Server.Database;
using Server.Database.Models;

namespace Server.Commands
{
    internal class SignUpCommand : RequestCommand
    {
        private IUserRepository _database;

        public SignUpCommand(CloudStorageClient client, IUserRepository repository) : base(client)
        {
            _database = repository;
        }

        protected override bool CanExecute(Request request, out string? errorMessage)
        {
            errorMessage = null;

            if (request == null || request.Args.Length == 0)
            {
                errorMessage = "Not correct request";
                
                return false;
            }

            string? username = request.Username;
            if (username == null || username.Length < 4 || username.Contains(' '))
            {
                errorMessage = "Username must be more than 4 characters and must not contain spaces";
                
                return false;
            }

            User? user = _database.GetByUsername(username);
            if (user != null)
            {
                errorMessage = "A user with the same username already exists";

                return false;
            }

            string password = request.Args[0];
            if (password.Length < 8)
            {
                errorMessage = "Password must be more than 8 characters";

                return false;
            }

            return true;
        }

        protected override void DoAction(Request request)
        {
            User user = new User(request.Username!, password: request.Args[0]);

            _database.Insert(user);
            _database.Save();

            Directory.CreateDirectory(user.Username);

            Response response = new Response(CommandStatus.Ok);
            _client.SendResponse(response, "The account created");
        }
    }
}
