using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VideoconferencingBackend.DTO.Group.Requests;

namespace VideoconferencingBackend.Models.DBModels
{
    public class Group
    {
        public int Id { get; set; }
        [Required]
        [StringLength(64, MinimumLength = 4, ErrorMessage = "Group name is in interval [4, 64]")]
        public string Name { get; set; }
        [StringLength(256, ErrorMessage = "Group description should be less than 256 symbols")]
        public string Description { get; set; }
        public bool? InCall { get; set; }
        public User Creator { get; set; }
        public string AvatarLink { get; set; }
        public ICollection<GroupUser> GroupUsers { get; set; }

        public static implicit operator Group(GroupCreateDto v)
        {
            return new Group{AvatarLink = v.AvatarLink, Name = v.Name, Description = v.Description};
        }

    }
}
