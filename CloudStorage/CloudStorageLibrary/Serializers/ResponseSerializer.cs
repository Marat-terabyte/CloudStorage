// MIT License
// Copyright (c) 2024 Marat

using CloudStorageLibrary.Commands;
using CloudStorageLibrary.Serializers.Exceptions;
using System.Text;

namespace CloudStorageLibrary.Serializers
{
    public static class ResponseSerializer
    {
        public static string Serialize(Response response)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append(response.Status.ToString() + '\n');
            stringBuilder.Append(response.DataLenght);
            stringBuilder.Append(" bytes");

            return stringBuilder.ToString();
        }

        public static Response Deserialize(string response)
        {
            Response DeserializedResponse = new Response();

            DeserializedResponse.Status = ReadCommandStatus(out int index, response);
            DeserializedResponse.DataLenght = ReadResponseLength(ref index, response);

            return DeserializedResponse;
        }

        private static CommandStatus ReadCommandStatus(out int index, string response)
        {
            index = 0;
            
            string commandStatus = StringSeparator.Separate(ref index, response, '\n').Replace(" ", "");

            return GetCommandStatus(commandStatus);
        }

        private static CommandStatus GetCommandStatus(string commandStatus)
        {
            switch (commandStatus)
            {
                case "Ok":
                    return CommandStatus.Ok;
                case "NotOk":
                    return CommandStatus.NotOk;
                default:
                    throw new NotSupportedStatus($"{commandStatus} is not supported status");
            }
        }

        private static long ReadResponseLength(ref int index, string response) => long.Parse(StringSeparator.Separate(ref index, response, ' '));
    }
}
