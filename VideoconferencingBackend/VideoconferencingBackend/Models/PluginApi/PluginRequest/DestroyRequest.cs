using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VideoconferencingBackend.Models.JanusApi;

namespace VideoconferencingBackend.Models.PluginApi.PluginRequest
{
    public class DestroyRequest : PluginBase<DestroyVideoroomBody>
    {
        public override DestroyVideoroomBody Body { get; set; }
    }

    public class DestroyVideoroomBody : PluginBodyBase
    {
        public override string Request => "join";
        public bool Permanent { get; set; }
        public string Secret { get; set; }

    }
}
