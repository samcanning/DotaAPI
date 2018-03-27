using Microsoft.AspNetCore.Mvc;
using DotaAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace DotaAPI.Controllers
{
    public class UserController : Controller
    {
        private DotaContext _context;
        public UserController(DotaContext context)
        {
            _context = context;
        }

        [Route("register")]
        public IActionResult Register()
        {
            return View();
        }

        [Route("register/submit")]
        public IActionResult RegisterSubmit(UserValidator model)
        {
            if(!ModelState.IsValid) return View("Register");
            if(_context.Users.SingleOrDefault(u => u.username == model.username) != null)
            {
                ModelState.AddModelError("username", "This username is already taken.");
                return View("Register");
            }
            PasswordHasher<User> hasher = new PasswordHasher<User>();
            User newUser = new User(){ username = model.username };
            newUser.password = hasher.HashPassword(newUser, model.password);
            _context.Users.Add(newUser);
            _context.SaveChanges();
            HttpContext.Session.SetString("username", _context.Users.Single(u => u.username == newUser.username).username);
            return RedirectToAction("Index", "Home");
        }

        [Route("login")]
        public IActionResult Login()
        {
            return View();
        }

        [Route("login/submit")]
        public IActionResult LoginSubmit(LoginValidator model)
        {
            if(!ModelState.IsValid) return View("Login");
            User thisUser = _context.Users.SingleOrDefault(u => u.username == model.username);
            if(thisUser == null)
            {
                ModelState.AddModelError("username", "No user found with this username.");
                return View("Login");
            }
            PasswordHasher<User> hasher = new PasswordHasher<User>();
            if(hasher.VerifyHashedPassword(thisUser, thisUser.password, model.password) == 0)
            {
                ModelState.AddModelError("password", "Incorrect password.");
                return View("Login");
            }
            HttpContext.Session.SetString("username", thisUser.username);
            return RedirectToAction("Index", "Home");
        }
    }
}