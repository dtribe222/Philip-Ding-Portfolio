using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using quiz.Model;

namespace quiz.Data
{
    public class QuizDBContext : DbContext
    {
        public QuizDBContext(DbContextOptions<QuizDBContext> options) : base(options) { 
        }
        public DbSet<User> Users { get; set; }

        public DbSet<Item> Items { get; set; }
        public DbSet<Admin> Admins { get; set; }

    }

}