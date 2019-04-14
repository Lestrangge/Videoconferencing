namespace VideoconferencingBackend.Models.Janus.PluginApi.PluginResponse
{
    public class UnpublishedResponse : PluginResponseBase
    {
        public UnpublishedPluginDataResponse Plugindata { get; set; }
    }

    public class UnpublishedPluginDataResponse : PluginResponseDataBase
    {
        public UnpublishedDataResponse Data { get; set; }
    }

    public class UnpublishedDataResponse : PluginResponseDataBase
    {
        public long? Unpublished { get; set; }
    }
}
