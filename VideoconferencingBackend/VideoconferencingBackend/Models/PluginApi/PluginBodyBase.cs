using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VideoconferencingBackend.Models
{
    public class PluginBodyBase
    {
        public virtual string Request { get; }
        public long Room { get; set; }
    }
}
