namespace VideoconferencingBackend.Models.Janus.PluginApi
{
    public class PluginBase<T> : JanusBase where T : PluginBodyBase
    {
        public override string Janus => "message";
        public virtual T Body { get; set; }
    }
}
