using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using quiz.Data;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using quiz.Model;
using Microsoft.EntityFrameworkCore;

namespace quiz.Data
{
    public class QuizRepo : IQuizRepo
    {
        private readonly QuizDBContext _dbContext;

        public QuizRepo(QuizDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public User GetUser(string e)
        {
            User user = _dbContext.Users.FirstOrDefault(o => o.UserName == e);
            return user;
        }
        public IEnumerable<User> GetAllUsers()
        {
            IEnumerable<User> users = _dbContext.Users.ToList<User>();
            return users;


        }
        public IEnumerable<Admin> GetAllAdmins()
        {
            IEnumerable<Admin> admins = _dbContext.Admins.ToList<Admin>();
            return admins;
        }
        public User AddUser(User user)
        {
            EntityEntry<User> e = _dbContext.Users.Add(user);
            User c = e.Entity;
            _dbContext.SaveChanges();
            return c;
        }
        public Item AddItem(Item item)
        {
            EntityEntry<Item> e = _dbContext.Items.Add(item);
            Item a = e.Entity;
            _dbContext.SaveChanges();
            return a;
        }
        public IEnumerable<Item> AllItems()
        {
            IEnumerable<Item> items = _dbContext.Items.ToList<Item>();
            return items;
        }

        public Item GetItem(Int32 id)
        {
            return _dbContext.Items.FirstOrDefault(x => x.Id == id);
        }
        public bool ValidLogin(string uname, string pass)
        {
            User c = _dbContext.Users.FirstOrDefault(e => e.UserName == uname && e.Password == pass);
            if (c == null)
                return false;
            else
                return true;
        }
        public bool ValidAdmin(string uname, string pass)
        {
            Admin c = _dbContext.Admins.FirstOrDefault(e => e.UserName == uname && e.Password == pass);
            if (c == null)
                return false;
            else
                return true;
        }
        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }
    }
}