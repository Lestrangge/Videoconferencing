namespace VideoconferencingBackend.Models.Janus.PluginApi.PluginResponse
{
    public class PluginErrorResponse : JanusBase
    {
        public long Sender { get; set; }
        public PluginErrorData Plugindata { get; set; }
    }

    public class PluginErrorData
    {
        public ErrorData Data { get; set; }
    }

    public class ErrorData
    {
        public string Error { get; set; }
        public int ErrorCode { get; set; }
        public string VideoRoom { get; set; }
    }
}
