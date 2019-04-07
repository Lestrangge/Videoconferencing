namespace VideoconferencingBackend.Models.Janus.PluginApi.PluginRequest
{
    public class CreateRequest : PluginBase<CreateRequestBody>
    {
    }

    public class CreateRequestBody : PluginBodyBase
    {
        public override string Request => "create";
        public bool Permanent => true;
        public string Secret { get; set; }
        public string Pin { get; set; }
        public bool IsPrivate { get; set; }
    }
}
