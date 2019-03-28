using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VideoconferencingBackend.DTO
{
    public class UserUpdateDto
    {
        public string Password { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}
