namespace VideoconferencingBackend.Models.Janus.PluginApi.PluginResponse
{
    public class PluginResponseBase : JanusBase
    {
        public long Sender { get; set; }
        public PluginResponsePluginDataBase Plugindata { get; set; }
    }

    public class PluginResponsePluginDataBase
    {
        public string Plugin { get; set; }
        public PluginResponseDataBase Data { get; set; }
    }

    public class PluginResponseDataBase
    {
        public string Videoroom { get; set; }
        public long? Room { get; set; }
    }

}
