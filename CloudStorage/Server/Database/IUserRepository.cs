// MIT License
// Copyright (c) 2024 Marat

using Server.Database.Models;

namespace Server.Database
{
    internal interface IUserRepository
    {
        IEnumerable<User> GetAll();
        User? GetById(int id);
        void Insert(User user);
        void Delete(int id);
        void Update(User user);
        void Save();
    }
}
