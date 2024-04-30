// MIT License
// Copyright (c) 2024 Marat

using CloudStorageLibrary.Commands;
using System.Xml;

namespace CloudStorageLibrary
{
    /// <summary>
    /// Contains the user's request
    /// </summary>
    public class Request
    {
        public string Username { get; set; }
        public Command Command { get; set; }
        public string[] Args { get; set; }

        public Request() { }

        public Request(string username, Command command, string[] args)
        {
            Username = username;
            Command = command;
            Args = args;
        }

        public Request(string username, Command command)
        {
            Username = username;
            Command = command;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as Request);
        }

        public bool Equals(Request? other)
        {
            return other != null &&
                other.Username == Username &&
                other.Command == Command &&
                Args.SequenceEqual(other.Args);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Username, Command, Args);
        }
    }
}
