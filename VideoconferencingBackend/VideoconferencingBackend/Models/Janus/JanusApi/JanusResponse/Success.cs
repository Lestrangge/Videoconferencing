namespace VideoconferencingBackend.Models.Janus.JanusApi.JanusResponse
{
    public class SuccessJanus : JanusBase
    {
        public SuccessJanusData Data { get; set; }
        public long SessionId { get; set; }
        public SuccessJanus()
        {
            Data = new SuccessJanusData();
        }

        public SuccessJanus(int id) : this()
        {
            Data.Id = id;
        }
    }

    public class SuccessJanusData
    {
        public int Id { get; set; }
    }
}
