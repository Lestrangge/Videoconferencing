using VideoconferencingBackend.Models.JanusApi;

namespace VideoconferencingBackend.Models
{
    public class PluginBase<T> : JanusBase where T : PluginBodyBase
    {
        public override string Janus
        {
            get { return "message"; }
        }
        public virtual T Body { get; set; }
    }
}
