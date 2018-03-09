using Newtonsoft.Json.Linq;

namespace DotaAPI.Models
{
    public class DisplaySpell
    {
        public int id {get;set;}
        public string name {get;set;}
        public string description {get;set;}
        public JObject details {get;set;}
        public string hero {get;set;}
        public int hero_id {get;set;}
        public bool ultimate {get;set;}

    }
}