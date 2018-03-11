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
    public class CreateController : Controller
    {
        private DotaContext _context;

        public CreateController(DotaContext context)
        {
            _context = context;
        }

        [Route("")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("create")]
        public IActionResult Create()
        {
            List<DisplaySpell> allSpells = new List<DisplaySpell>();
            foreach(Spell s in _context.Spells)
            {
                allSpells.Add(Convert(s));
            }
            ViewBag.heroes = _context.Heroes.ToList();
            ViewBag.regulars = allSpells.Where(s => s.ultimate == false).ToList();
            ViewBag.ultimates = allSpells.Where(s => s.ultimate == true).ToList();
            return View();
        }

        [Route("create/submit")]
        [HttpPost]
        public IActionResult Submit(NewHeroValidator model)
        {
            List<DisplaySpell> allSpells = new List<DisplaySpell>();
            foreach(Spell s in _context.Spells)
            {
                allSpells.Add(Convert(s));
            }
            ViewBag.heroes = _context.Heroes.ToList();
            ViewBag.regulars = allSpells.Where(s => s.ultimate == false).ToList();
            ViewBag.ultimates = allSpells.Where(s => s.ultimate == true).ToList();
            if(model.hero_id == 0)
            {
                ModelState.AddModelError("hero_id", "You must select a base for your hero.");
            }
            if(model.spell_1 == 0 || model.spell_2 == 0 || model.spell_3 == 0)
            {
                ModelState.AddModelError("spell_1", "You must select three regular spells.");
            }
            else if(model.spell_1 == model.spell_2 || model.spell_1 == model.spell_3 || model.spell_2 == model.spell_3)
            {
                ModelState.AddModelError("spell_1", "Cannot use the same spell twice.");
            }
            if(model.spell_4 == 0)
            {
                ModelState.AddModelError("spell_4", "Your hero needs an ultimate.");
            }
            if(ModelState.IsValid)
            {
                New_Hero hero = new New_Hero(){
                    name = model.name,
                    hero_id = model.hero_id,
                    spell_1_id = model.spell_1,
                    spell_2_id = model.spell_2,
                    spell_3_id = model.spell_3,
                    spell_4_id = model.spell_4
                };
                if(model.bio.Length > 0) hero.bio = model.bio;
                _context.Add(hero);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.bio = model.bio;
            return View("Create");
        }

        public DisplaySpell Convert(Spell input)
        {
            DisplaySpell display = new DisplaySpell(){
                id = input.id,
                name = input.name,
                description = input.description,
                details = JObject.Parse(input.details),
                hero_id = input.hero_id,
                hero = _context.Heroes.SingleOrDefault(h => h.id == input.hero_id).name
            };
            if(input.ultimate == 1) display.ultimate = true;
            else display.ultimate = false;
            return display;
        }

        public NewHeroDisplay ConvertHero(int id)
        {
            New_Hero newHero = _context.New_Heroes.SingleOrDefault(n => n.id == id);
            Hero hero = _context.Heroes.SingleOrDefault(h => h.id == newHero.hero_id);
            NewHeroDisplay display = new NewHeroDisplay()
            {
                id = newHero.id,
                name = newHero.name,
                attribute = hero.attribute,
                intelligence = hero.intelligence,
                agility = hero.agility,
                strength = hero.strength,
                attack = hero.attack,
                speed = hero.speed,
                armor = hero.armor,
                bio = newHero.bio,
                attack_type = hero.attack_type,
                attack_range = hero.attack_range,
                base_hero = hero.name
            };
            List<DisplaySpell> spells = new List<DisplaySpell>();
            spells.Add(Convert(_context.Spells.SingleOrDefault(s => s.id == newHero.spell_1_id)));
            spells.Add(Convert(_context.Spells.SingleOrDefault(s => s.id == newHero.spell_2_id)));
            spells.Add(Convert(_context.Spells.SingleOrDefault(s => s.id == newHero.spell_3_id)));
            spells.Add(Convert(_context.Spells.SingleOrDefault(s => s.id == newHero.spell_4_id)));
            display.spells = spells;
            return display;
        }

        [Route("test")]
        public IActionResult Test()
        {
            return Json(ConvertHero(2));
        }
    }
}