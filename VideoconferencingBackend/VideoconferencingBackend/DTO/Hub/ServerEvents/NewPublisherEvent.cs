using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VideoconferencingBackend.DTO.User.Responses;
using VideoconferencingBackend.Models.Janus.PluginApi;

namespace VideoconferencingBackend.DTO.Hub.ServerEvents
{
    public class NewPublisherEvent
    {
        public long HandleId { get; set; }
        public Jsep Answer { get; set; }
        public UserFoundDto User { get; set; }

        public NewPublisherEvent(long id, Jsep jsep, Models.DBModels.User user)
        {
            this.User = new UserFoundDto(user);
            this.Answer = jsep;
            HandleId = id;
        }

        public NewPublisherEvent(long id, Jsep jsep, string login)
        {
            this.Answer = jsep;
            HandleId = id;
            User = new UserFoundDto(login);
        }
    }
}
