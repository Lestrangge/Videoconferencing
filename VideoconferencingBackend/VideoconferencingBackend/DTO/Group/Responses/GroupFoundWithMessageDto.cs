using VideoconferencingBackend.DTO.Message.Response;

namespace VideoconferencingBackend.DTO.Group.Responses
{
    public class GroupFoundWithMessageDto : GroupFoundDto
    {
        public GroupMessageDto LastMessage { get; set; }

        public GroupFoundWithMessageDto(Models.DBModels.Group group, Models.DBModels.Message message) : base(group)
        {
            LastMessage = new GroupMessageDto(message);
        }
    }
}
