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
            if(HttpContext.Session.GetString("admin") == "true") return RedirectToAction("Main");
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
        public IActionResult UpdateHeroList()
        {
            if(HttpContext.Session.GetString("admin") != "true") return RedirectToAction("Index");
            return View(_context.Heroes.ToList());
        }
        public IActionResult SelectHero(int id)
        {
            return RedirectToAction("UpdateHero", new {id = id});
        }

        [Route("admin/updatehero/{id}")]
        public IActionResult UpdateHero(int id)
        {
            if(HttpContext.Session.GetString("admin") != "true") return RedirectToAction("Index");
            return View(_context.Heroes.SingleOrDefault(h => h.id == id));
        }

        [HttpPost]
        public IActionResult SubmitHeroChanges(Hero model)
        {
            Hero thisHero = _context.Heroes.SingleOrDefault(h => h.id == model.id);
            thisHero.name = model.name;
            thisHero.attribute = model.attribute;
            thisHero.intelligence = model.intelligence;
            thisHero.agility = model.agility;
            thisHero.strength = model.strength;
            thisHero.attack = model.attack;
            thisHero.speed = model.speed;
            thisHero.armor = model.armor;
            thisHero.bio = model.bio;
            thisHero.attack_type = model.attack_type;
            thisHero.attack_range = model.attack_range;
            thisHero.img = model.img;
            _context.Update(thisHero);
            _context.SaveChanges();
            return RedirectToAction("UpdateHero", new {id = model.id});
        }

        [Route("admin/updatespell")]
        public IActionResult UpdateSpellList()
        {
            if(HttpContext.Session.GetString("admin") != "true") return RedirectToAction("Index");
            return View(_context.Spells.ToList());
        }

        public IActionResult SelectSpell(int id)
        {
            return RedirectToAction("UpdateSpell", new {id = id});
        }

        [Route("admin/updatespell/{id}")]
        public IActionResult UpdateSpell(int id)
        {
            if(HttpContext.Session.GetString("admin") != "true") return RedirectToAction("Index");
            return View(_context.Spells.SingleOrDefault(s => s.id == id));
        }

        [HttpPost]
        public IActionResult SubmitSpellChanges(Spell model)
        {
            Spell thisSpell = _context.Spells.SingleOrDefault(s => s.id == model.id);
            thisSpell.name = model.name;
            thisSpell.description = model.description;
            thisSpell.details = model.details;
            thisSpell.hero_id = model.hero_id;
            thisSpell.ultimate = model.ultimate;
            thisSpell.img = model.img;
            _context.Update(thisSpell);
            _context.SaveChanges();
            return RedirectToAction("UpdateSpell", new {id = model.id});
        }

        [Route("admin/delete/{type}")]
        public IActionResult DeleteList(string type)
        {
            if(HttpContext.Session.GetString("admin") != "true") return RedirectToAction("Index");
            if(type == "hero")
            {
                ViewBag.type = "Hero";
                ViewBag.list = _context.Heroes.ToList();
                return View();
            }
            if(type == "spell")
            {
                ViewBag.type = "Spell";
                ViewBag.list = _context.Spells.ToList();
                return View();
            }
            return View("Main");
        }

        [HttpPost]
        [Route("admin/delete/confirm")]
        public IActionResult SelectDelete(string type, int id, string username, string password)
        {
            PasswordHasher<User> hasher = new PasswordHasher<User>();
            User thisUser = _context.Users.SingleOrDefault(u => u.username == username);
            if(thisUser == null) return RedirectToAction("Main");
            if(hasher.VerifyHashedPassword(thisUser, thisUser.password, password) == 0) return RedirectToAction("Main");
            type = type.ToLower();
            ViewBag.type = type;
            ViewBag.id = id;
            if(type == "hero")
            {
                ViewBag.name = _context.Heroes.Single(h => h.id == id).name;
            }
            if(type == "spell")
            {
                ViewBag.name = _context.Spells.Single(s => s.id == id).name;
            }
            return View();
        }

        [HttpPost]
        public IActionResult ConfirmDelete(string type, int id, string yesButton, string noButton)
        {
            if(yesButton == "Yes")
            {
                if(type == "hero")
                {
                    Hero toDelete = _context.Heroes.Single(h => h.id == id);
                    _context.Remove(toDelete);
                }
                if(type == "spell")
                {
                    Spell toDelete = _context.Spells.Single(s => s.id == id);
                    _context.Remove(toDelete);
                }
                _context.SaveChanges();
            }
            return RedirectToAction("Main");
        }

        [Route("admin/deletenew")]
        public IActionResult DeleteNewHeroList()
        {
            List<New_Hero> heroes = _context.New_Heroes.ToList();
            return View(heroes);
        }

        [HttpPost]
        [Route("admin/deletenew/submit")]
        public IActionResult DeleteNew(int id, string username, string password)
        {
            PasswordHasher<Admin> hasher = new PasswordHasher<Admin>();
            Admin thisAdmin = _context.Admins.SingleOrDefault(a => a.username == username);
            if(thisAdmin == null) return RedirectToAction("DeleteNewHeroList");
            if(hasher.VerifyHashedPassword(thisAdmin, thisAdmin.password, password) == 0) return RedirectToAction("DeleteNewHeroList");
            New_Hero heroToDelete = _context.New_Heroes.Single(n => n.id == id);
            List<Vote> votesToDelete = _context.Votes.Where(v => v.new_hero_id == heroToDelete.id).ToList();
            if(votesToDelete.Count > 0)
            {
                foreach(Vote v in votesToDelete)
                {
                    _context.Remove(v);
                }
            }
            _context.SaveChanges();
            _context.Remove(heroToDelete);
            _context.SaveChanges();
            return RedirectToAction("Main");
        }
        
    }
}