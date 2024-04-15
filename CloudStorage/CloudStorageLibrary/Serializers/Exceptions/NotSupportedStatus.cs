// MIT License
// Copyright (c) 2024 Marat

namespace CloudStorageLibrary.Serializers.Exceptions
{
    public class NotSupportedStatus : Exception
    {
        public NotSupportedStatus() { }

        public NotSupportedStatus(string message) : base(message) { }

        public NotSupportedStatus(string message,  Exception innerException) : base(message, innerException) { }
    }
}
