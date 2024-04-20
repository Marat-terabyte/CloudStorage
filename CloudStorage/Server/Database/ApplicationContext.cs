// MIT License
// Copyright (c) 2024 Marat

using Microsoft.EntityFrameworkCore;
using Server.Configuration;
using Server.Database.Models;

namespace Server.Database
{
    internal class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public ApplicationContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(ConfigReader.ReadConfig()?.DatabaseConnectionString);
        }
    }
}
