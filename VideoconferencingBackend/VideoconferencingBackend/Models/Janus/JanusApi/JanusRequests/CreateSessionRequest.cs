namespace VideoconferencingBackend.Models.Janus.JanusApi.JanusRequests
{
    public class CreateSessionRequest : JanusBase
    {
        public new string Janus => "create";

        public CreateSessionRequest(string transaction)
        {
            Transaction = transaction;
        }
        public CreateSessionRequest() { }
    }
}
