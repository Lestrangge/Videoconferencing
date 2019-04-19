using VideoconferencingBackend.DTO.Message.Response;

namespace VideoconferencingBackend.DTO.Group.Responses
{
    public class GroupFoundWithMessageDto : GroupFoundDto
    {
        public GroupMessageDto LastMessage { get; set; }
        public int Participants { get; set; }

        public GroupFoundWithMessageDto(Models.DBModels.Group group, Models.DBModels.Message message, int participants = 0) : base(group)
        {
            LastMessage = message != null ? new GroupMessageDto(message) : new GroupMessageDto();
            Participants = participants;
        }
    }
}
