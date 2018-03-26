using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace DotaAPI.Models
{
    public abstract class Converter
    {

        public static DisplaySpell Convert(Spell input)
        {
            DisplaySpell display = new DisplaySpell(){
                id = input.id,
                name = input.name,
                description = input.description,
                details = JObject.Parse(input.details),
                hero_id = input.hero_id,
                img = "http://cdn.dota2.com/apps/dota2/images/abilities/" + input.img + "_hp2.png"
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
                attack_range = temp.attack_range,
                img = "http://cdn.dota2.com/apps/dota2/images/heroes/" + temp.img + "_vert.jpg"
            };
            List<DisplaySpell> displays = new List<DisplaySpell>();
            foreach(Spell s in spells)
            {
                displays.Add(Convert(s));
            }
            result.spells = displays;
            return result;
        }
        public static NewHeroDisplay ConvertHero(New_Hero hero, Hero baseHero, Spell spell1, Spell spell2, Spell spell3, Spell spell4)
        {
            NewHeroDisplay display = new NewHeroDisplay()
            {
                id = hero.id,
                name = hero.name,
                attribute = baseHero.attribute,
                intelligence = baseHero.intelligence,
                agility = baseHero.agility,
                strength = baseHero.strength,
                attack = baseHero.attack,
                speed = baseHero.speed,
                armor = baseHero.armor,
                bio = hero.bio,
                attack_type = baseHero.attack_type,
                attack_range = baseHero.attack_range,
                base_hero = baseHero.name
            };
            List<DisplaySpell> spells = new List<DisplaySpell>();
            spells.Add(Converter.Convert(spell1));
            spells.Add(Converter.Convert(spell2));
            spells.Add(Converter.Convert(spell3));
            spells.Add(Converter.Convert(spell4));
            display.spells = spells;
            return display;
        } 

        public static string DetailsString(Spell input)
        {
            string output = "";
            for(int i = 0; i < input.details.Length-1; i++)
            {
                if(input.details[i] == '{') continue;
                else if(input.details[i] == '_') output += ' ';
                else if(input.details[i] == '"') continue;
                else output += input.details[i];
            }
            return output;
        }
    }
}