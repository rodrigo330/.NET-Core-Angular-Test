using System.ComponentModel.DataAnnotations;

namespace web.api.Dtos
{
    public class UserForRegisterDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [StringLength(8,MinimumLength = 4,ErrorMessage = "You must specify password betwen 4 and 8 characters")]
        public string Password { get; set; }
    }
}