using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VideoconferencingBackend.Models.PluginApi.PluginRequest
{
    public class JoinRequest : PluginBase<JoinBody>
    {
    }

    public class JoinBody : PluginBodyBase
    {
        public override string Request => "join";
    }
}
