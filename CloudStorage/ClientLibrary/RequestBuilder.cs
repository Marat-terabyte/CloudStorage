// MIT License
// Copyright (c) 2024 Marat

using CloudStorageLibrary;
using CloudStorageLibrary.Commands;

namespace ClientLibrary
{
    internal static class RequestBuilder
    {
        public static Request Build(Command command, string[] args)
        {
            Request request = new Request();
            request.SessionId = UserInfo.SessionId;
            request.Username = UserInfo.Username;
            request.Command = command;
            request.Args = args;

            return request;
        }
    }
}
