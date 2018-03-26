using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace DotaAPI.Models
{
    public class New_Hero_Creator : New_Hero
    {
        // public int id {get;set;}
        // [Required(ErrorMessage = "Your hero must have a name.")]
        // public string name {get;set;}
        // public string bio {get;set;}
        // public int hero_id {get;set;}
        // public int spell_1_id {get;set;}
        // public int spell_2_id {get;set;}
        // public int spell_3_id {get;set;}
        // public int spell_4_id {get;set;}
        // public string img {get;set;}
        public IFormFile file {get;set;}
    }
}