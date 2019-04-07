namespace VideoconferencingBackend.Models.Janus.PluginApi
{
    public class PluginBodyBase
    {
        public virtual string Request { get; }
        public long Room { get; set; }
    }
}
