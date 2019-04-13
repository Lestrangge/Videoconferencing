namespace VideoconferencingBackend.Models.Janus.PluginApi
{
    public class PluginRequestBase<T> : JanusBase where T : PluginRequestBodyBase
    {
        public override string Janus => "message";
        public virtual T Body { get; set; }
        public long? HandleId { get; set; }
    }
}
