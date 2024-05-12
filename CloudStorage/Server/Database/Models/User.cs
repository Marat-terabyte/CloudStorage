// MIT License
// Copyright (c) 2024 Marat

namespace Server.Database.Models
{
    /// <summary>
    /// Contains information about user
    /// </summary>
    internal class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public User() { }

        public User(int id, string username, string password)
        {
            Id = id;
            Username = username;
            Password = password;
        }

        public User(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}
