using System.Collections.Generic;

namespace VideoconferencingBackend.Models.Janus.PluginApi.PluginResponse
{
    public class JoinAndConfigureResponse : PluginResponseBase
    {
        public Jsep Jsep { get; set; }
        public JoinAndConfigureResponsePluginData Plugindata { get; set; }
    }

    public class JoinAndConfigureResponsePluginData : PluginResponsePluginDataBase
    {
        public JoinAndConfigureResponseData Data { get; set; }
    }

    public class JoinAndConfigureResponseData : PluginResponseDataBase
    {
        public IEnumerable<Publisher> Publishers { get; set; }
    }
}
