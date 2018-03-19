using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DotaAPI.Models;
using Newtonsoft.Json.Linq;

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
            //should create version that includes hero name
            return Json(Converter.Convert(thisSpell));
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
            return View(Converter.Convert(thisSpell));
        }

    }
}
