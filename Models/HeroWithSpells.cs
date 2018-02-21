using System.Collections.Generic;

namespace DotaAPI.Models
{
    public class HeroWithSpells : Hero
    {
        public List<Spell> spells {get;set;}
    }
}