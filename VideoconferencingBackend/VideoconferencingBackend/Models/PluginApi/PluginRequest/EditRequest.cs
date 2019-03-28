using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VideoconferencingBackend.Models.JanusApi;

namespace VideoconferencingBackend.Models.PluginApi.PluginRequest
{
    public class EditRequest : PluginBase<EditVideoroomBody>
    {
        public override EditVideoroomBody Body { get; set; }
    }

    public class EditVideoroomBody : PluginBodyBase
    {
        public override string Request => "edit";
        public string NewDescription { get; set; }
        public string NewPin { get; set; }
        public bool NewIsPrivate { get; set; }
        public bool NewPublishers { get; set; }
        public bool Permanent { get; set; }
        public string Secret { get; set; }
    }
}
