using System;
using VideoconferencingBackend.DTO.User.Responses;

namespace VideoconferencingBackend.DTO.Message.Response
{
    public class GroupMessageDto
    {
        public string Text { get; set; }
        public string GroupGuid { get; set; }
        public UserFoundDto User { get; set; }
        public string Time { get; set; }

        public GroupMessageDto(Models.DBModels.Message message)
        {
            Text = message.Text;
            GroupGuid = message.Group.GroupGuid;
            User = new UserFoundDto(message.Sender);
            Time = message.Time.ToString("yyyy-MM-ddTHH:mm:ssZ");
        }
    }
}
