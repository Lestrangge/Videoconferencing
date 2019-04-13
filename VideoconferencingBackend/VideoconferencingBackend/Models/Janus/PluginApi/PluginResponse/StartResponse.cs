namespace VideoconferencingBackend.Models.Janus.PluginApi.PluginResponse
{
    public class StartResponse : PluginResponseBase
    {
        public StartResponsePluginData Plugindata { get; set; }
    }

    public class StartResponsePluginData : PluginResponsePluginDataBase
    {
        public StartResponseData Data { get; set; }
    }

    public class StartResponseData : PluginResponseDataBase
    {
        public string Started { get; set; }
    }

}
