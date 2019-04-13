using System.Collections.Generic;

namespace VideoconferencingBackend.Models.Janus.PluginApi.PluginResponse
{
    public class NewAvailablePublisherResponse : PluginResponseBase
    {
        public NewAvailablePublisherResponsePluginData Plugindata { get; set; }
    }

    public class NewAvailablePublisherResponsePluginData : PluginResponsePluginDataBase
    {
        public NewAvailablePublisherResponseData Data { get; set; }
    }

    public class NewAvailablePublisherResponseData : PluginResponseDataBase
    {
        public IEnumerable<Publisher> Publishers { get; set; }
    }

    public class Publisher
    {
        public long? Id { get; set; }
        public string Display { get; set; }
        public string AudioCodec { get; set; }
        public string VideoCodec { get; set; }
    }
}
