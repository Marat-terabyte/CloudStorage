// MIT License
// Copyright (c) 2024 Marat

using CloudStorageLibrary.Commands;
using CloudStorageLibrary.Serializers.Exceptions;
using System.Text;

namespace CloudStorageLibrary.Serializers
{
    /// <summary>
    /// Provides functionality for serializing and deserializing requests
    /// </summary>
    public static class RequestSerializer
    {
        /// <summary> Converts <see cref="Request"/> to the string </summary>
        /// <param name="request"> The value to serialize </param>
        public static string Serialize(Request request)
        {
            StringBuilder stringBuilder = new StringBuilder();
            
            stringBuilder.Append($"SessionId: {request.SessionId}\r\n");
            stringBuilder.Append($"Username: {request.Username}\r\n");
            stringBuilder.Append($"Command: {request.Command.ToString()}\r\n");
            SerializeRequestArgs(request, stringBuilder);

            return stringBuilder.ToString();
        }

        private static string SerializeRequestArgs(Request request, StringBuilder stringBuilder)
        {
            stringBuilder.Append("Args: ");

            if (request.Args == null)
                return "";

            // Add the first not null or empty argument
            int i = 0;
            for (; i < stringBuilder.Length && i < request.Args.Length; i++)
            {
                var arg = request.Args[i];
                if (!string.IsNullOrEmpty(arg))
                {
                    stringBuilder.Append(arg);
                    i++;

                    break;
                }
            }

            for (; i < request.Args.Length; i++)
            {
                string arg = request.Args[i];
                if (string.IsNullOrWhiteSpace(arg))
                    continue;
                
                stringBuilder.Append(", " + arg);
            }

            return stringBuilder.ToString();
        }

        /// <summary> Converts string request to <see cref="Request"/> </summary>
        /// <param name="request"> The value to deserialize </param>
        /// <exception cref="NotSupportedCommand"/>
        public static Request Deserialize(string request)
        {
            Request deserializedRequest = new Request();

            int i = 0;
            while (i < request.Length)
            {
                string requestField = "";
                requestField = StringSeparator.Separate(ref i, request, ':');

                var field = typeof(Request).GetProperty(requestField);
                if (field != null)
                {
                    string parameter = StringSeparator.Separate(ref i, request, '\r');

                    object? value = null;
                    if (field.Name == "SessionId")
                        value = ReadSessionId(parameter);
                    else if (field.Name == "Username")
                        value = ReadUsername(parameter);
                    else if (field.Name == "Command")
                        value = GetCommand(parameter);
                    else if (field.Name == "Args")
                        value = ReadArgs(parameter);

                    field.SetValue(deserializedRequest, value);
                    i++; // skip the char '\n'
                }
            }

            return deserializedRequest;
        }

        private static long? ReadSessionId(string id)
        {
            bool parsed = long.TryParse(id, out var sessionId);
            if (parsed)
                return sessionId;

            return null;
        }

        private static string? ReadUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return null;

            return username.Trim();
        }

        private static Command GetCommand(string command)
        {
            switch (command.Trim())
            {
                case "Upload":
                    return Command.Upload;
                case "Download":
                    return Command.Download;
                case "Remove":
                    return Command.Remove;
                case "SignIn":
                    return Command.SignIn;
                case "SignUp":
                    return Command.SignUp;
                case "SignOut":
                    return Command.SignOut;
                case "List":
                    return Command.List;
                default:
                    throw new NotSupportedCommand($"{command} is not supported command");
            }
        }

        private static string[] ReadArgs(string str)
        {
            List<string> args = new List<string>();

            int index = 0;
            while (index < str.Length)
            {
                string arg = StringSeparator.Separate(ref index, str, ", ").Trim();
                if (!string.IsNullOrEmpty(arg))
                    args.Add(arg);
            }

            return args.ToArray();
        }
    }
}
