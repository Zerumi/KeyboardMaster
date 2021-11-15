﻿// This code & software is licensed under the Creative Commons license. You can't use AMWE trademark 
// You can use & improve this code by keeping this comments
// (or by any other means, with saving authorship by Zerumi and PizhikCoder retained)
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;

namespace KeyboardMaster_Server
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Role userRole = new Role { Id = 1, Name = Role.GlobalUserRole };


            modelBuilder.Entity<Role>().HasData(new Role[] { userRole });
            base.OnModelCreating(modelBuilder);
        }
    }

    public class Role
    {
        public const string GlobalUserRole = "user";
        public const string GlobalUserGroup = "User";

        public int Id { get; set; }
        public string Name { get; set; }
        public List<User> Users { get; set; }
        public Role()
        {
            Users = new List<User>();
        }
    }
    public class User
    {
        public int Id { get; set; }

        public int? RoleId { get; set; }
        public Role Role { get; set; }
    }
}