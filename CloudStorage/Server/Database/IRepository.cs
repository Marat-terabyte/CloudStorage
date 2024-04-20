// MIT License
// Copyright (c) 2024 Marat

namespace Server.Database
{
    internal interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T? GetById(int id);
        void Insert(T entity);
        void Delete(int id);
        void Update(T entity);
        void Save();
    }
}
