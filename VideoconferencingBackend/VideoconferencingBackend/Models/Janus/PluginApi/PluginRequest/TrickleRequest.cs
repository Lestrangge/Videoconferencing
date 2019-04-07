namespace VideoconferencingBackend.Models.Janus.PluginApi.PluginRequest
{
    public class TrickleRequest : JanusBase
    {
        public override string Janus => "trickle";
        public TrickleCandidate Candidate { get; set; }
    }

    public class TrickleCandidate
    {
        public int SdpMLineIndex { get; set; }
        public string SdpMid { get; set; }
        public string Candidate { get; set; }
    }
}
