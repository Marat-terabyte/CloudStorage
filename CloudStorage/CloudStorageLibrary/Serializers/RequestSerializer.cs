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

            stringBuilder.Append(request.Username + '\n');
            stringBuilder.Append(request.Command.ToString() + '\n');
            SerializeRequestArgs(request, stringBuilder);

            return stringBuilder.ToString();
        }

        private static string SerializeRequestArgs(Request request, StringBuilder stringBuilder)
        {
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
        public static Request? Deserialize(string request)
        {
            Request DeserializedRequest = new Request();

            try
            {
                DeserializedRequest.Username = ReadUsername(out int index, request);
                DeserializedRequest.Command = ReadCommand(ref index, request);
                DeserializedRequest.Args = ReadArgs(ref index, request);
            }
            catch
            {
                return null;
            }

            return DeserializedRequest;
        }

        private static string ReadUsername(out int index, string request)
        {
            index = 0;
            return StringSeparator.Separate(ref index, request, '\n');
        }
        private static Command ReadCommand(ref int index, string request) => GetCommand(StringSeparator.Separate(ref index, request, '\n'));

        private static Command GetCommand(string command)
        {
            switch (command)
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

        private static string[] ReadArgs(ref int index, string request)
        {
            List<string> args = new List<string>();

            while (index < request.Length)
            {
                args.Add(StringSeparator.Separate(ref index, request, ", "));
            }

            return args.ToArray();
        }
    }
}
