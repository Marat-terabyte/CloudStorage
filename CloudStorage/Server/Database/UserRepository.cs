// MIT License
// Copyright (c) 2024 Marat

using Microsoft.EntityFrameworkCore;
using Server.Database.Models;

namespace Server.Database
{
    /// <summary>
    /// Represents Repository pattern
    /// </summary>
    internal class UserRepository : IRepository<User>, IDisposable
    {
        private ApplicationContext _context;
        private bool _disposed = false;

        public UserRepository(ApplicationContext context)
        {
            _context = context;
        }

        public IEnumerable<User> GetAll()
        {
            return _context.Users.ToList();
        }

        public User? GetById(int id)
        {
            return _context.Users.Find(id);
        }

        public void Insert(User user)
        {
            _context.Users.Add(user);
        }

        public void Delete(int id)
        {
            User? user = _context.Users.Find(id);
            if (user != null)
                _context.Users.Remove(user);
        }

        public void Update(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            
            this._disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
