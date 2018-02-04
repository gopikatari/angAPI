using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.DTO
{
    public class UserForRegisterDTO
    {
        [Required]
        public string username { get; set; }
        [Required]
        [StringLength(8,MinimumLength=4 ,ErrorMessage=" Password must be minimum 4 t0 8 characters length")]
        public string  password { get; set; }
    }
}