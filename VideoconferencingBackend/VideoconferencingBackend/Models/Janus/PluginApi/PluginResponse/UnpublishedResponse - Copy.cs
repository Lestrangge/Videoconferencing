namespace VideoconferencingBackend.Models.Janus.PluginApi.PluginResponse
{
    public class UnpublishResponse : PluginResponseBase
    {
        public UnpublishPluginDataResponse Plugindata { get; set; }
    }

    public class UnpublishPluginDataResponse : PluginResponseDataBase
    {
        public UnpublishDataResponse Data { get; set; }
    }

    public class UnpublishDataResponse : PluginResponseDataBase
    {
        public string Unpublished { get; set; }
    }
}
