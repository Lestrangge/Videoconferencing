namespace VideoconferencingBackend.Models.Janus.JanusApi.JanusRequests
{
    public class AttachPluginRequest : JanusBase
    {
        public new string Janus => "attach";

        public string Plugin => "janus.plugin.videoroom";
    }
}
