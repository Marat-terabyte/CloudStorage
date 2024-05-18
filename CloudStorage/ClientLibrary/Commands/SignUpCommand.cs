// MIT License
// Copyright (c) 2024 Marat

using CloudStorageLibrary;
using CloudStorageLibrary.Commands;

namespace ClientLibrary.Commands
{
    public class SignUpCommand : ClientCommand
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public SignUpCommand(string username, string password)
        {
            Username = username;
            Password = password;
        }

        public bool Execute(out string? message)
        {
            Request request = RequestBuilder.Build(Command.SignUp, Username, [Password]);
            Client.SendRequest(request);

            Response response = Client.ReceiveResponse(out string? data);
            message = data;

            if (response.Status == CommandStatus.Ok)
                return true;
            else
                return false;
        }
    }
}
