// MIT License
// Copyright (c) 2024 Marat

using CloudStorageLibrary;
using CloudStorageLibrary.Commands;
using Server.Database;
using Server.Database.Models;

namespace Server.Commands
{
    internal class SignInCommand : RequestCommand
    {
        private IUserRepository _database;

        public bool IsAuthorized { get; private set; }

        public SignInCommand(CloudStorageServer server, IUserRepository repository) : base(server)
        {
            IsAuthorized = false;
            _database = repository;
        }

        protected override bool CanExecute(Request request, out string? errorMessage)
        {
            errorMessage = null;

            if (request == null)
            {
                errorMessage = "Not correct request";

                return false;
            }

            if (string.IsNullOrWhiteSpace(request.Username))
            {
                errorMessage = "You should send your username";

                return false;
            }

            if (request.Args.Length == 0)
            {
                errorMessage = "You should send your password";

                return false;
            }

            return true;
        }

        protected override void DoAction(Request request)
        {
            var users = _database.GetAll();

            string username = request.Username!;
            string password = request.Args[0];

            User? user = users.Where(u => u.Username == username).FirstOrDefault();
            if (user != null && user.Password == password)
            {
                IsAuthorized = true;

                Response response = new Response(CommandStatus.Ok);
                _server.SendResponse(response, "You logged in");
            }
            else
            {
                Response response = new Response(CommandStatus.NotOk);
                _server.SendResponse(response, "Wrong login or password");
            }
        }
    }
}
