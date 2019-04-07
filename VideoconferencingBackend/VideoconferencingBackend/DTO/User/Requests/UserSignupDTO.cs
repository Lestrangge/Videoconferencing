using System.ComponentModel.DataAnnotations;

namespace VideoconferencingBackend.DTO.User.Requests
{
    public class UserSignupDto
    {
        [Required]
        public string Login { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}
