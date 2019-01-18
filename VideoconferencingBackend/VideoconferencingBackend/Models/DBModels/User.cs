using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VideoconferencingBackend.Models.DBModels
{
    public class User
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Login is required")]
        [StringLength(32, MinimumLength = 4, ErrorMessage = "Login length is in range [4,32]")]
        public string Login { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [RegularExpression("^(?=.*[A-Za-z])(?=.*\\d)[A-Za-z\\d]{5,}$")]
        public string Password { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string ImageLink { get; set; }
        public long? SessionId { get; set; }
        public long? HandleId { get; set; }
        public Role Role { get; set; }
    }
}
