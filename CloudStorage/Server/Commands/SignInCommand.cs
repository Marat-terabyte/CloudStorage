﻿// MIT License
// Copyright (c) 2024 Marat

using CloudStorageLibrary;
using CloudStorageLibrary.Commands;
using Server.Database;
using Server.Database.Models;

namespace Server.Commands
{
    internal class SignInCommand : RequestCommand
    {
        private long _sessionId;
        private IUserRepository _database;

        public bool IsAuthorized { get; private set; }

        public SignInCommand(CloudStorageClient client, long sessionId, IUserRepository repository) : base(client)
        {
            IsAuthorized = false;
            _sessionId = sessionId;
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
            string username = request.Username!;
            string password = request.Args[0];

            User? user = _database.GetByUsername(username);
            if (user != null && user.Password == password)
            {
                IsAuthorized = true;

                Response response = new Response(CommandStatus.Ok);
                _client.SendResponse(response, _sessionId.ToString()); // Send session id to the client
            }
            else
            {
                Response response = new Response(CommandStatus.NotOk);
                _client.SendResponse(response, "Wrong login or password");
            }
        }
    }
}
