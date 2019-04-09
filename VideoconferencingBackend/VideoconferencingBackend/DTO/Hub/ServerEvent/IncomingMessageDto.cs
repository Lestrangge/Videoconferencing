using System;
using VideoconferencingBackend.DTO.User.Responses;
using VideoconferencingBackend.Models.DBModels;

namespace VideoconferencingBackend.DTO.Hub.ServerResponse
{
    public class IncomingMessageDto
    {

        public IncomingMessageDto(Models.DBModels.Message message)
        {
            User = new UserFoundDto(message.Sender);
            Text = message.Text;
            GroupName = message.Group.Name;
            DateTime = message.Time;
        }

        public string GroupName { get; set; }
        public UserFoundDto User { get; set; }
        public DateTime DateTime { get; set; }
        public string Text { get; set; }
    }
}
