using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using VideoconferencingBackend.DTO.User.Requests;

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
        public string UserGuid { get; set; }
        public long? SessionId { get; set; }
        public long? HandleId { get; set; }
        public Role Role { get; set; }
        public string AvatarLink { get; set; }
        public string ConnectionId { get; set; }
        public ICollection<GroupUser> GroupUsers { get; set; }

        public User()
        {

        }

        public User(UserLoginDto credentials)
        {
            Login = credentials.Login;
            Password = credentials.Password;
        }

        public User(UserSignupDto credentials)
        {
            Name = credentials.Name;
            Login = credentials.Login;
            Surname = credentials.Surname;
            Password = credentials.Password;
        }
    }
}
