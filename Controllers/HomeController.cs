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
            return View();
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
            return Json(thisSpell);
        }


        // public HeroWithSpells addSpells(Hero temp, List<Spell> spells)
        // {
        //     HeroWithSpells result = new HeroWithSpells(){
        //         id = temp.id,
        //         name = temp.name,
        //         attribute = temp.attribute,
        //         intelligence = temp.intelligence,
        //         agility = temp.agility,
        //         strength = temp.strength,
        //         attack = temp.attack,
        //         speed = temp.speed,
        //         armor = temp.armor,
        //         bio = temp.bio,
        //         attack_type = temp.attack_type,
        //         attack_range = temp.attack_range
        //     };
        //     List<DisplaySpell> displays = new List<DisplaySpell>();
        //     foreach(Spell s in spells)
        //     {
        //         displays.Add(Convert(s));
        //     }
        //     result.spells = displays;
        //     return result;
        // }        

        // public DisplaySpell Convert(Spell input)
        // {
        //     DisplaySpell display = new DisplaySpell(){
        //         id = input.id,
        //         name = input.name,
        //         description = input.description,
        //         details = JObject.Parse(input.details),
        //         hero_id = input.hero_id
        //     };
        //     if(input.ultimate == 1) display.ultimate = true;
        //     else display.ultimate = false;
        //     return display;
        // }

    }
}
