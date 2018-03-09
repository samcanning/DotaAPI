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
            List<DisplaySpell> regulars = allSpells.Where(s => s.ultimate == false).ToList();
            List<DisplaySpell> ultimates = allSpells.Where(s => s.ultimate == true).ToList();
            List<List<DisplaySpell>> container = new List<List<DisplaySpell>>();
            container.Add(regulars);
            container.Add(ultimates);
            return View(container);
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
    }
}