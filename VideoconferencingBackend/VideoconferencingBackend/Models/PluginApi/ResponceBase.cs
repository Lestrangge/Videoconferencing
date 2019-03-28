using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VideoconferencingBackend.Models.JanusApi;

namespace VideoconferencingBackend.Models.PluginApi
{
    public class ResponceBase : JanusBase
    {
        public override string Janus
        {
            get { return "event"; }
        }

    }
}
