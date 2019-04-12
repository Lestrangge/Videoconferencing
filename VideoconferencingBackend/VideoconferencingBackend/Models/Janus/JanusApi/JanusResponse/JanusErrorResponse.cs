namespace VideoconferencingBackend.Models.Janus.JanusApi.JanusResponse
{
    public class JanusErrorResponse : JanusBase
    {
        public JanusErrorBody Error { get; set; }
    }
    public class JanusErrorBody{
        public int Code { get; set; }
        public string Reason { get; set; }
    }
}
