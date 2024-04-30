// MIT License
// Copyright (c) 2024 Marat

using CloudStorageLibrary.Commands;

namespace CloudStorageLibrary
{
    /// <summary>
    /// Contains the server's response
    /// </summary>
    public class Response
    {
        public CommandStatus Status { get; set; }

        /// <summary> Data length in bytes </summary>
        public long DataLenght { get; set; }

        public Response() { }

        public Response(CommandStatus status, long dataLenght)
        {
            Status = status;
            DataLenght = dataLenght;
        }

        public Response(CommandStatus status)
        {
            Status = status;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as Response);
        }

        public bool Equals(Response? other)
        {
            return other != null &&
                other.Status == Status &&
                other.DataLenght == DataLenght;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Status, DataLenght);
        }
    }
}
