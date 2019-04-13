namespace VideoconferencingBackend.Models.Janus.PluginApi.PluginRequest
{
    public class CreateRequest : PluginRequestBase<CreateRequestBody>
    {
    }

    public class CreateRequestBody : PluginRequestBodyBase
    {
        public override string Request => "create";
        public bool Permanent => true;
        public string Secret { get; set; }
        public string Pin { get; set; }
        public bool IsPrivate { get; set; }
    }
}
