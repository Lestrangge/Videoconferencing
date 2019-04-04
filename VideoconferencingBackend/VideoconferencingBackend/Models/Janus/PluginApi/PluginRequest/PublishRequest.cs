namespace VideoconferencingBackend.Models.Janus.PluginApi.PluginRequest
{
    public class PublishRequest : PluginBase<PublishRequestBody>
    {
        public Jsep Jsep { get; set; }
    }

    public class PublishRequestBody : PluginBodyBase
    {
        public override string Request => "publish";
        public bool Audio { get; set; }
        public bool Video { get; set; }
        public bool Data { get; set; }
        public bool Record => true;

    }
}
