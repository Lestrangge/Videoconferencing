namespace VideoconferencingBackend.Models.Janus.PluginApi.PluginResponse
{
    public class MediaResponse : JanusBase
    {
        public string Type { get; set; }
        public bool Receiving { get; set; }
        public long Sender { get; set; }
    }
}
