// MIT License
// Copyright (c) 2024 Marat

using CloudStorageLibrary;
using CloudStorageLibrary.Commands;

namespace ClientLibrary.Commands
{
    public class SignInCommand : ClientCommand
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public SignInCommand(string username, string password)
        {
            Username = username;
            Password = password;
        }

        public bool Execute(out string? message)
        {
            Request request = RequestBuilder.Build(Command.SignIn, Username, [Password]);
            Client.SendRequest(request);

            Response response = Client.ReceiveResponse(out string? data);
            if (response.Status == CommandStatus.Ok)
            {
                if (!long.TryParse(data, out long id))
                {
                    message = data;

                    return false;
                }

                message = null;
                UserInfo.Username = Username;
                UserInfo.SessionId = id;

                return true;
            }
            else
            {
                message = data;

                return false;
            }
        }
    }
}
