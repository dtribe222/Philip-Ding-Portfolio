using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using quiz.Model;

namespace quiz.Data
{
    public interface IQuizRepo
    {
        public User GetUser(string e);
        IEnumerable<User> GetAllUsers();
        IEnumerable<Admin> GetAllAdmins();
        User AddUser(User user);
        Item AddItem(Item item);
        IEnumerable<Item> AllItems();
        Item GetItem(Int32 id);
        public bool ValidLogin(string userName, string password);
        public bool ValidAdmin(string userName, string password);
        void SaveChanges();
    }
}