using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VideoconferencingBackend.Models.Janus.PluginApi;

namespace VideoconferencingBackend.DTO.Hub.ServerEvents
{
    public class NewPublisherEvent
    {
        public long HandleId { get; set; }
        public Jsep Jsep { get; set; }
    }
}
