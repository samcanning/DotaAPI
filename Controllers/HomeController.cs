using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DotaAPI.Models;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Http;

namespace DotaAPI.Controllers
{
    public class HomeController : Controller
    {

        private DotaContext _context;

        public HomeController(DotaContext context)
        {
            _context = context;
        }

        [Route("")]
        public IActionResult Index()
        {
            ViewBag.name = HttpContext.Session.GetString("username");
            return View();
        }

        [Route("list")]
        public IActionResult List()
        {
            return View(_context.New_Heroes.ToList());
        }

        [Route("hero/{id}")]
        public IActionResult HeroPage(int id)
        {
            New_Hero heroToDisplay = _context.New_Heroes.SingleOrDefault(h => h.id == id);
            if(heroToDisplay == null) return RedirectToAction("List");
            Hero baseHero = _context.Heroes.SingleOrDefault(h => h.id == heroToDisplay.hero_id);
            Spell spell1 = _context.Spells.SingleOrDefault(s => s.id == heroToDisplay.spell_1_id);
            Spell spell2 = _context.Spells.SingleOrDefault(s => s.id == heroToDisplay.spell_2_id);
            Spell spell3 = _context.Spells.SingleOrDefault(s => s.id == heroToDisplay.spell_3_id);
            Spell spell4 = _context.Spells.SingleOrDefault(s => s.id == heroToDisplay.spell_4_id);
            ViewBag.base_id = heroToDisplay.hero_id;
            if(heroToDisplay.img == null)
            {
                ViewBag.img = baseHero.img;
                ViewBag.user_image = false;
            }
            else
            {
                ViewBag.user_image = true;
                ViewBag.img = heroToDisplay.img;
            }
            if(heroToDisplay.user_id != null)
            {
                User creator = _context.Users.SingleOrDefault(u => u.id == heroToDisplay.user_id);
                ViewBag.username = creator.username;
                ViewBag.user_id = creator.id;
            }
            ViewBag.loggedUser = HttpContext.Session.GetString("username");
            decimal rating = 0;
            List<Vote> votes = _context.Votes.Where(v => v.new_hero_id == id).ToList();
            if(votes.Count == 0)
            {
                ViewBag.rating = null;
            }
            else
            {
                foreach(Vote v in votes)
                {
                    rating += v.value;
                }
                ViewBag.rating = rating / votes.Count;
                ViewBag.voteCount = votes.Count;
            }
            if(heroToDisplay == _context.New_Heroes.Last()) ViewBag.position = "last";
            else if(heroToDisplay == _context.New_Heroes.First()) ViewBag.position = "first";
            else ViewBag.position = "middle";
            return View(Converter.ConvertHero(heroToDisplay, baseHero, spell1, spell2, spell3, spell4));
        }

        [Route("api/hero/{id}")]
        public IActionResult Hero(int id)
        {
            Hero thisHero = _context.Heroes.SingleOrDefault(h => h.id == id);
            HeroWithSpells result = Converter.addSpells(thisHero, _context.Spells.Where(s => s.hero_id == id).ToList());
            // HeroWithSpells result = addSpells(thisHero, _context.Spells.Where(s => s.hero_id == id).ToList());
            return Json(result);
        }

        [Route("api/spell/{id}")]
        public IActionResult Spell(int id)
        {
            Spell thisSpell = _context.Spells.SingleOrDefault(s => s.id == id);
            var display = Converter.Convert(thisSpell);
            display.hero = _context.Heroes.Single(h => h.id == thisSpell.hero_id).name;
            return Json(display);
        }

        [Route("about")]
        public IActionResult About()
        {
            return View();
        }

        [Route("base/hero/{id}")]
        public IActionResult BaseHeroPage(int id)
        {
            Hero thisHero = _context.Heroes.SingleOrDefault(h => h.id == id);
            if(thisHero == null) return RedirectToAction("Index");
            List<Spell> spells = _context.Spells.Where(s => s.hero_id == id).ToList();
            ViewBag.new_heroes = _context.New_Heroes.Where(n => n.hero_id == id).ToList();
            ViewBag.img = thisHero.img;
            if(thisHero == _context.Heroes.Last()) ViewBag.position = "last";
            else if(thisHero == _context.Heroes.First()) ViewBag.position = "first";
            else ViewBag.position = "middle";
            return View(Converter.addSpells(thisHero, spells));
        }

        [Route("base/spell/{id}")]
        public IActionResult SpellPage(int id)
        {
            Spell thisSpell = _context.Spells.SingleOrDefault(s => s.id == id);
            if(thisSpell == null) return RedirectToAction("Index");
            ViewBag.hero = _context.Heroes.SingleOrDefault(h => h.id == thisSpell.hero_id).name;
            ViewBag.details_string = Converter.DetailsString(thisSpell);
            ViewBag.new_heroes = _context.New_Heroes.Where(n => n.spell_1_id == id || n.spell_2_id == id || n.spell_3_id == id || n.spell_4_id == id).ToList();
            ViewBag.img = thisSpell.img;
            return View(Converter.Convert(thisSpell));
        }

        [HttpPost]
        public IActionResult VoteUp(int id, string user)
        {
            User thisUser = _context.Users.Single(u => u.username == user);
            Vote existingVote = _context.Votes.SingleOrDefault(v => v.new_hero_id == id && v.user_id == thisUser.id);
            if(existingVote == null)
            {
                Vote newVote = new Vote()
                {
                   new_hero_id = id,
                   user_id = thisUser.id,
                   value = 1 
                };
                _context.Add(newVote);
            }
            else
            {
                existingVote.value = 1;
                _context.Update(existingVote);
            }
            _context.SaveChanges();
            return RedirectToAction("HeroPage", new { id = id });
        }

        [HttpPost]
        public IActionResult VoteDown(int id, string user)
        {
            User thisUser = _context.Users.Single(u => u.username == user);
            Vote existingVote = _context.Votes.SingleOrDefault(v => v.new_hero_id == id && v.user_id == thisUser.id);
            if(existingVote == null)
            {
                Vote newVote = new Vote()
                {
                   new_hero_id = id,
                   user_id = thisUser.id,
                   value = 0
                };
                _context.Add(newVote);
            }
            else
            {
                existingVote.value = 0;
                _context.Update(existingVote);
            }
            _context.SaveChanges();
            return RedirectToAction("HeroPage", new { id = id });
        }

    }
}
