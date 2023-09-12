using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using quiz.Data;
using quiz.Dto;
using quiz.Model;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace quiz.Controllers
{
    [Route("api")]
    [ApiController]
    public class QuizController : Controller
    {
        private readonly IQuizRepo _repository;
        public QuizController(IQuizRepo repository)
        {
            _repository = repository;
        }
        [HttpPost("Register")]
        public ActionResult<string> RegisterUser(User user)
        {
            IEnumerable<User> users = _repository.GetAllUsers();
            IEnumerable<Admin> admins = _repository.GetAllAdmins();
            User NewUser = new User { UserName = user.UserName, Password = user.Password };
            foreach (var person in admins)
            {
                if (person.UserName == user.UserName)
                {
                    return Ok("Username not available.");
                }
            }
            foreach (var person in users)
            {
                if (person.UserName == user.UserName)
                {
                    return Ok("Username not available.");
                }
            }
            User addedUser = _repository.AddUser(NewUser);
            return Ok("User successfully registered.");
        }
        [HttpGet("ListItems")]
        public ActionResult<IEnumerable<Item>> ListItems()
        {
            IEnumerable<Item> items = _repository.AllItems();
            IEnumerable<Item> sortedList = items.OrderBy(q => q.StartBid).ThenBy(q => q.Id).ToList();
            IEnumerable<Item> activeList = sortedList.Where(q => q.State == "active").ToList();
            return Ok(activeList);
        }

        [HttpGet("GetItemPhoto/{id}")]
        public ActionResult<Byte[]> GetItemPhoto(Int32 id)
        {
            string filename = System.IO.Directory.GetFiles(@".\Photos\", $"{id}.*").FirstOrDefault();
            if (filename != null)
            {
                string ext = Path.GetExtension(filename);
                if (ext == ".jpeg")
                {
                    return File(System.IO.File.ReadAllBytes(filename), "image/jpeg");

                }
                else if (ext == ".gif")
                {
                    return File(System.IO.File.ReadAllBytes(filename), "image/gif");
                }
                else if (ext == ".png")
                {
                    return File(System.IO.File.ReadAllBytes(filename), "image/png");
                }
            }

            return File(System.IO.File.ReadAllBytes(@".\Photos\logo.pdf"), "application/pdf", "downloaded-file.pdf");

        }
        [HttpGet("GetItem/{id}")]
        public ActionResult<Item> GetItem(Int32 id)
        {
            Item item = _repository.GetItem(id);
            return Ok(item);
        }

        [Authorize(AuthenticationSchemes = "Authentication")]
        [Authorize(Policy = "UserOnly")]
        [HttpPost("AddItem")]
        public ActionResult<Item> AddItem(ItemInput item)
        {
            ClaimsIdentity ci = HttpContext.User.Identities.FirstOrDefault();
            Claim c = ci.FindFirst("userName");
            string username = c.Value;
            User user = _repository.GetUser(username);
            float sbid = 0;
            if (item.StartBid != null)
            {
                sbid = (float)item.StartBid;
            }
            Item i = new Item
            {
                Owner = username,
                Title = item.Title,
                Description = item.Description,
                StartBid = sbid,
                CurrentBid = 0,
                State = "active"
            };
            Item addedItem = _repository.AddItem(i);
            return i;
        }
        [Authorize(AuthenticationSchemes = "Authentication")]
        [Authorize(Policy = "AdminOnly")]
        [HttpGet("ListItemsAdmin")]
        public ActionResult<IEnumerable<Item>> AllItemsAdmin()
        {
            IEnumerable<Item> items = _repository.AllItems();
            IEnumerable<Item> sortedList = items.OrderBy(q => q.Id).ToList();
            return Ok(sortedList);
        }
        [Authorize(AuthenticationSchemes = "Authentication")]
        [Authorize(Policy = "Both")]
        [HttpGet("CloseAuction/{id}")]
        public ActionResult<string> CloseAuction(Int32 id)
        {
            ClaimsIdentity ci = HttpContext.User.Identities.FirstOrDefault();
            Claim c = ci.FindFirst("userName");
            if (c == null) {
                c = ci.FindFirst("admin");
                string username = c.Value;
                User user = _repository.GetUser(username);
                IEnumerable<Item> items = _repository.AllItems();
                Item i = items.FirstOrDefault(e => e.Id == id);
                if (i == null)
                {
                    return "Auction does not exist.";
                }
                else
                {
                    i.State = "closed";
                    _repository.SaveChanges();
                    return "Auction Closed.";
                }
            }
            else
            {
                string username = c.Value;
                User user = _repository.GetUser(username);
                IEnumerable<Item> items = _repository.AllItems();
                Item i = items.FirstOrDefault(e => e.Id == id);
                if (i == null)
                {
                    return "Auction does not exist.";
                }
                if (i.Owner != username)
                {
                    return "You are not the owner of the auction.";
                }
                else
                {
                    i.State = "closed";
                    _repository.SaveChanges();
                    return "Auction Closed.";
                }
            }
        }
    }
}