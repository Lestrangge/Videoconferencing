using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VideoconferencingBackend.DTO.Group.Requests
{
    public class GroupCreateDto
    {
        [Required]
        [StringLength(64, MinimumLength = 4, ErrorMessage = "Group name is in interval [4, 64]")]
        public string Name { get; set; }
        public string Description { get; set; }
        public string AvatarLink { get; set; }
        public IEnumerable<string> Users { get; set; }
    }
}
