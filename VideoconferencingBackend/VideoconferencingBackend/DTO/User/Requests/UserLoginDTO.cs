using System.ComponentModel.DataAnnotations;

namespace VideoconferencingBackend.DTO.User.Requests
{
    public class UserLoginDto
    {
        [Required]
        public string Login { get; set; }

        public string Password { get; set; }

    }
}
