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

        [Route("create")]
        public IActionResult Create()
        {
            List<DisplaySpell> allSpells = new List<DisplaySpell>();
            foreach(Spell s in _context.Spells)
            {
                allSpells.Add(Converter.Convert(s));
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
                allSpells.Add(Converter.Convert(s));
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

        

        [Route("test")]
        public IActionResult Test()
        {
            return Json(Converter.ConvertHero(2));
        }
    }
}