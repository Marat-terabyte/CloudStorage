// MIT License
// Copyright (c) 2024 Marat

using CloudStorageLibrary;
using CloudStorageLibrary.Commands;
using Server.Commands;
using Server.Database;
using Server.Database.Models;
using System.Net.Sockets;

namespace Server.Session
{
    internal class UserSession
    {
        private bool _disposed;

        public DateTime Created {  get; }

        public bool IsAuthorizied { get; private set; }
        public long SessionID { get; }

        public string? Username { get; set; }
        public string? BasePath
        {
            get
            {
                if (Username == null)
                    return null;

                return Path.Combine(Directory.GetCurrentDirectory(), Username);
            }
        }

        public IUserRepository Database { get; set; }

        public UserSession(long id, DateTime createdTime , IUserRepository repository)
        {
            IsAuthorizied = false;
            SessionID = id;
            Database = repository;
            Created = createdTime;
        }

        public void Execute(CloudStorageClient client, Request request)
        {
            try
            {
                ExecuteCommand(client, request);
            }
            catch (Exception ex)
            {
                if (ex is not SocketException)
                {
                    Console.WriteLine(ex.ToString());
                    string errorMessage = "An error occurred during runtime";
                    Response response = new Response(CommandStatus.NotOk);

                    client.SendResponse(response, errorMessage);
                }
            }
            finally
            {
                client.Dispose();
            }
        }

        private Request ReceiveRequest(CloudStorageClient client)
        {
            Request? request = null;
            while (request == null)
            {
                request = client.ReceiveRequest();
            }

            return request;
        }

        private void ExecuteCommand(CloudStorageClient client, Request request)
        {
            CommandHandler commandHandler = new CommandHandler(client, SessionID, Database, BasePath!);

            RequestCommand? command = commandHandler.GetCommand(request);
            if (command == null)
            {
                Response response = new Response(CommandStatus.NotOk);
                string errorMessage = "The command does not exist";
                client.SendResponse(response, errorMessage);

                return;
            }

            if (!IsAuthorizied && command is not SignInCommand && command is not SignUpCommand)
            {
                Response response = new Response(CommandStatus.NotOk);
                string errorMessage = "You should be logged in";

                client.SendResponse(response, errorMessage);
            }
            else
            {
                command.Execute(request);
                if (command is SignInCommand)
                {
                    this.Username = request.Username;
                    this.IsAuthorizied = ((SignInCommand) command).IsAuthorized;
                }
            }
        }
    }
}
