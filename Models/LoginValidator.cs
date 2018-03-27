using System.ComponentModel.DataAnnotations;

namespace DotaAPI.Models
{
    public class LoginValidator
    {
        [Required(ErrorMessage = "Must enter username.")]
        public string username {get;set;}
        [Required(ErrorMessage = "Must enter password.")]
        [DataType(DataType.Password)]
        public string password {get;set;}
    }
}