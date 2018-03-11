using System.ComponentModel.DataAnnotations;

namespace DotaAPI.Models
{
    public class NewHeroValidator
    {
        [Required(ErrorMessage = "Your hero must have a name.")]
        public string name {get;set;}
        public string bio {get;set;}
        public int hero_id {get;set;}
        public int spell_1 {get;set;}
        public int spell_2 {get;set;}
        public int spell_3 {get;set;}
        public int spell_4 {get;set;}
        
    }
}