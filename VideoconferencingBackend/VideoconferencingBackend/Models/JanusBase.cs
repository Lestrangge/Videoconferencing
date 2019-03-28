using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VideoconferencingBackend.Models.JanusApi
{
    public class JanusBase
    {
        public virtual string Janus { get; set; }
        public string Transaction { get; set; }
    }
}
