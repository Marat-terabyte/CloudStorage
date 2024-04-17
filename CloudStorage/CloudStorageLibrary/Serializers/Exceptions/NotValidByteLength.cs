// MIT License
// Copyright (c) 2024 Marat

namespace CloudStorageLibrary.Serializers.Exceptions
{
    public class NotValidByteLength : Exception
    {
        public NotValidByteLength() { }

        public NotValidByteLength(string message) : base(message) { }

        public NotValidByteLength(string message,  Exception innerException) : base(message, innerException) { }
    }
}
