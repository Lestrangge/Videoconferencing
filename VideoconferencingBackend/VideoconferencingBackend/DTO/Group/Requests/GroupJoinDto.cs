using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VideoconferencingBackend.DTO.Group.Requests
{
    public class GroupJoinDto
    {
        public string GroupGuid { get; set; }
        public string UserGuid { get; set; }
    }

}
