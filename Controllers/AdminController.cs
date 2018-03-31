using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DotaAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json.Linq;

namespace DotaAPI.Controllers
{
    public class AdminController : Controller
    {
        private DotaContext _context;
        public AdminController(DotaContext context)
        {
            _context = context;
        }

        [Route("admin")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("admin/login")]
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            Admin thisAdmin = _context.Admins.SingleOrDefault(a => a.username == username);
            if(thisAdmin == null) return RedirectToAction("Index");
            PasswordHasher<Admin> hasher = new PasswordHasher<Admin>();
            if(hasher.VerifyHashedPassword(thisAdmin, thisAdmin.password, password) != 0)
            {
                HttpContext.Session.SetString("admin", "true");
                return RedirectToAction("Main");
            }
            return RedirectToAction("Index");
        }

        [Route("admin/main")]
        public IActionResult Main()
        {
            if(HttpContext.Session.GetString("admin") != "true") return RedirectToAction("Index");
            return View();
        }

        [Route("admin/addhero")]
        public IActionResult AddHero()
        {
            if(HttpContext.Session.GetString("admin") != "true") return RedirectToAction("Index");
            return View();
        }

        [Route("admin/addhero/create")]
        [HttpPost]
        public IActionResult CreateHero(Hero newHero)
        {
            Hero heroToAdd = new Hero()
            {
                name = newHero.name,
                attribute = newHero.attribute,
                intelligence = newHero.intelligence,
                agility = newHero.agility,
                strength = newHero.strength,
                attack = newHero.attack,
                speed = newHero.speed,
                armor = newHero.armor,
                bio = newHero.bio,
                attack_range = newHero.attack_range,
                attack_type = newHero.attack_type,
                img = newHero.img
            };
            _context.Add(heroToAdd);
            _context.SaveChanges();
            return RedirectToAction("AddHero");
        }

        [Route("admin/addspell")]
        public IActionResult AddSpell()
        {
            if(HttpContext.Session.GetString("admin") != "true") return RedirectToAction("Index");
            return View();
        }

        [Route("admin/addspell/create")]
        [HttpPost]
        public IActionResult CreateSpell(Spell newSpell)
        {
            Spell spellToAdd = new Spell()
            {
                name = newSpell.name,
                description = newSpell.description,
                details = newSpell.details,
                hero_id = newSpell.hero_id,
                ultimate = newSpell.ultimate,
                img = newSpell.img
            };
            _context.Add(spellToAdd);
            _context.SaveChanges();
            return RedirectToAction("AddSpell");
        }

        [Route("admin/updatehero")]
        public IActionResult UpdateHero()
        {
            if(HttpContext.Session.GetString("admin") != "true") return RedirectToAction("Index");
            return View();
        }

        [Route("admin/updatespell")]
        public IActionResult UpdateSpell()
        {
            if(HttpContext.Session.GetString("admin") != "true") return RedirectToAction("Index");
            return View();
        }
    }
}