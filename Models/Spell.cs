namespace DotaAPI.Models
{
    public class Spell
    {
        public int id {get;set;}
        public string name {get;set;}
        public string description {get;set;}
        public string details {get;set;}
        public int hero_id {get;set;}
        public int ultimate {get;set;}
        public string img {get;set;}
    }
}