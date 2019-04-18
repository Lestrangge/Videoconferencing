using VideoconferencingBackend.DTO.Group.Responses;
using VideoconferencingBackend.DTO.User.Responses;

namespace VideoconferencingBackend.DTO.Hub.ServerEvents
{
    public class CallStartedDto
    {
        public GroupFoundDto Group { get; set; }
        public UserFoundDto User { get; set; }

        public CallStartedDto(Models.DBModels.User user, Models.DBModels.Group group)
        {
            Group = new GroupFoundDto(group);
            User = new UserFoundDto(user);
        }
    }
}
