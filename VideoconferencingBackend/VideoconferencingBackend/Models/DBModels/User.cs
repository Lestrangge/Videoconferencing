using System;
using System.ComponentModel.DataAnnotations;
using VideoconferencingBackend.DTO;

namespace VideoconferencingBackend.Models.DBModels
{
    public class User
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Login is required")]
        [StringLength(32, MinimumLength = 4, ErrorMessage = "Login length is in range [4,32]")]
        public string Login { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public long? SessionId { get; set; }
        public long? HandleId { get; set; }
        public Role Role { get; set; }

        public static explicit operator User(UserLoginDTO v)
        {
            return new User{Login = v.Login, Password = v.Password};
        }

        public static explicit operator User(UserSignupDTO v)
        {
            return new User{Password = v.Password, Name = v.Name, Login = v.Login, Surname = v.Surname};
        }
    }
}
