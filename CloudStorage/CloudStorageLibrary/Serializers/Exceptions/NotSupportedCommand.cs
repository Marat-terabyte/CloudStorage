// MIT License
// Copyright (c) 2024 Marat

namespace CloudStorageLibrary.Serializers.Exceptions
{
    public class NotSupportedCommand : Exception
    {
        public NotSupportedCommand() { }

        public NotSupportedCommand(string message) : base(message) { }

        public NotSupportedCommand(string message, Exception innerException) : base(message, innerException) { }
    }
}
