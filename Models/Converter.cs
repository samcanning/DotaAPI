using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace DotaAPI.Models
{
    public abstract class Converter
    {

        private static DotaContext _context;

        public static DisplaySpell Convert(Spell input)
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

        public static HeroWithSpells addSpells(Hero temp, List<Spell> spells)
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
                attack_range = temp.attack_range
            };
            List<DisplaySpell> displays = new List<DisplaySpell>();
            foreach(Spell s in spells)
            {
                displays.Add(Convert(s));
            }
            result.spells = displays;
            return result;
        }
        public static NewHeroDisplay ConvertHero(int id)
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
            spells.Add(Converter.Convert(_context.Spells.SingleOrDefault(s => s.id == newHero.spell_1_id)));
            spells.Add(Converter.Convert(_context.Spells.SingleOrDefault(s => s.id == newHero.spell_2_id)));
            spells.Add(Converter.Convert(_context.Spells.SingleOrDefault(s => s.id == newHero.spell_3_id)));
            spells.Add(Converter.Convert(_context.Spells.SingleOrDefault(s => s.id == newHero.spell_4_id)));
            display.spells = spells;
            return display;
        } 
    }
}