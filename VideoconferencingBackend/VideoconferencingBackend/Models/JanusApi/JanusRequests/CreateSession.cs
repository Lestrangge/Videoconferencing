namespace VideoconferencingBackend.Models.JanusApi.JanusRequests
{
    public class CreateSession : JanusBase
    {
        public new string Janus => "create";

        public CreateSession(string transaction)
        {
            Transaction = transaction;
        }
        public CreateSession() { }
    }
}
