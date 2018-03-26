using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DotaAPI.Models;
using Newtonsoft.Json.Linq;
using System.IO;
using Amazon.S3;
using Amazon.S3.Transfer;

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
                try
                {
                    allSpells.Add(Converter.Convert(s));
                }
                catch
                {
                    System.Console.WriteLine("failed to convert " + s.name);
                }
            }
            ViewBag.heroes = _context.Heroes.ToList();
            ViewBag.regulars = allSpells.Where(s => s.ultimate == false).ToList();
            ViewBag.ultimates = allSpells.Where(s => s.ultimate == true).ToList();
            return View();
        }

        [Route("create/submit")]
        [HttpPost]
        public IActionResult Submit(New_Hero_Creator model)
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
            if(model.spell_1_id == 0 || model.spell_2_id == 0 || model.spell_3_id == 0)
            {
                ModelState.AddModelError("spell_1_id", "You must select three regular spells.");
            }
            else if(model.spell_1_id == model.spell_2_id || model.spell_1_id == model.spell_3_id || model.spell_2_id == model.spell_3_id)
            {
                ModelState.AddModelError("spell_1_id", "Cannot use the same spell twice.");
            }
            if(model.spell_4_id == 0)
            {
                ModelState.AddModelError("spell_4_id", "Your hero needs an ultimate.");
            }
            if(_context.New_Heroes.SingleOrDefault(n => n.name == model.name) != null)
            {
                ModelState.AddModelError("name", "This name is already in use.");
            }
            if(ModelState.IsValid)
            {
                New_Hero hero = new New_Hero(){
                    name = model.name,
                    hero_id = model.hero_id,
                    spell_1_id = model.spell_1_id,
                    spell_2_id = model.spell_2_id,
                    spell_3_id = model.spell_3_id,
                    spell_4_id = model.spell_4_id
                };
                if(model.bio != null) hero.bio = model.bio;
                if(model.file != null)
                {
                    TransferUtility transfer = new TransferUtility(Credentials.AccessKey, Credentials.SecretKey, Amazon.RegionEndpoint.USWest2);
                    using(var stream = new MemoryStream())
                    {
                        model.file.CopyTo(stream);
                        transfer.Upload(stream, "dhcimages", model.file.FileName);
                        hero.img = model.file.FileName;
                    }
                }
                _context.Add(hero);
                _context.SaveChanges();
                
                return RedirectToAction("HeroPage", "Home", new {id = _context.New_Heroes.SingleOrDefault(n => n.name == hero.name).id});
            }
            ViewBag.bio = model.bio;
            return View("Create");
        }
    }
}