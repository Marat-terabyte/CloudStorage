﻿// MIT License
// Copyright (c) 2024 Marat

using CloudStorageLibrary.Commands;
using CloudStorageLibrary.Serializers.Exceptions;
using System.Text;

namespace CloudStorageLibrary.Serializers
{
    /// <summary>
    /// Provides functionality for serializing and deserializing respons
    /// </summary>
    public static class ResponseSerializer
    {
        /// <summary> Converts <see cref="Response"/> to the string </summary>
        /// <param name="response">The value to convert</param>
        public static string Serialize(Response response)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append(response.Status.ToString() + '\n');
            stringBuilder.Append(response.DataLenght);
            stringBuilder.Append(" bytes");

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Converts string response to <see cref="Response"/>
        /// </summary>
        /// <param name="response"> The value to convert </param>
        /// <exception cref="NotSupportedCommand"/>
        /// <exception cref="NotValidByteLength"/>
        public static Response? Deserialize(string response)
        {
            Response DeserializedResponse = new Response();

            try
            {
                DeserializedResponse.Status = ReadCommandStatus(out int index, response);
                DeserializedResponse.DataLenght = ReadResponseLength(ref index, response);
            }
            catch
            {
                return null;
            }

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

        private static long ReadResponseLength(ref int index, string response)
        {
            bool isPasrsed = long.TryParse(StringSeparator.Separate(ref index, response, ' '), out long result);
            if (isPasrsed)
                return result;
            else
                throw new NotValidByteLength($"Data length is not {nameof(Int64)}");
        }
    }
}
