﻿using System;
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
        public IActionResult Index()
        {
            List<Hero> allHeroes = _context.Heroes.ToList();
            List<HeroWithSpells> result = new List<HeroWithSpells>();
            foreach(Hero h in allHeroes)
            {
                List<Spell> spells = _context.Spells.Where(s => s.hero_id == h.id).ToList();
                result.Add(addSpells(h, spells));
            }
            return Json(result);
        }

        [Route("hero/{id}")]
        public IActionResult Hero(int id)
        {
            Hero thisHero = _context.Heroes.SingleOrDefault(h => h.id == id);
            HeroWithSpells result = addSpells(thisHero, _context.Spells.Where(s => s.hero_id == id).ToList());
            return Json(result);
        }

        [Route("spell/{id}")]
        public IActionResult Spell(int id)
        {
            Spell thisSpell = _context.Spells.SingleOrDefault(s => s.id == id);
            //should create version that includes hero name
            return Json(thisSpell);
        }


        public HeroWithSpells addSpells(Hero temp, List<Spell> spells)
        {
            HeroWithSpells result = new HeroWithSpells(){
                id = temp.id,
                name = temp.name,
                attribute = temp.attribute,
                intelligence = temp.intelligence,
                agility = temp.agility,
                strength = temp.strength,
                attack = temp.attack,
                speed = temp.speed,
                armor = temp.armor,
                bio = temp.bio,
                attack_type = temp.attack_type,
                sight_range = temp.sight_range,
                attack_range = temp.attack_range,
                missile_speed = temp.missile_speed,
                version = temp.version,
                spells = spells
            };
            return result;
        }

        [Route("test/{id}")]
        public IActionResult Test(int id)
        {
            Spell selected = _context.Spells.Single(s => s.id == id);
            return Json(Convert(selected));
        }

        public DisplaySpell Convert(Spell input)
        {
            DisplaySpell display = new DisplaySpell(){
                id = input.id,
                name = input.name,
                description = input.description,
                details = JObject.Parse(input.details),
                hero_id = input.hero_id
            };
            if(input.ultimate == 1) display.ultimate = true;
            else display.ultimate = false;
            return display;
        }

    }
}
