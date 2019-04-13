namespace VideoconferencingBackend.Models.Janus.PluginApi
{
    public class PluginRequestBodyBase
    {
        public virtual string Request { get; }
        public long Room { get; set; }
    }
}
