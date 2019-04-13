namespace VideoconferencingBackend.Models.Janus.PluginApi.PluginRequest
{
    public class DestroyRequest : PluginRequestBase<DestroyVideoroomRequestBody>
    {
        public DestroyVideoroomRequestBody RequestBody { get; set; }
    }

    public class DestroyVideoroomRequestBody : PluginRequestBodyBase
    {
        public override string Request => "join";
        public bool Permanent { get; set; }
        public string Secret { get; set; }

    }
}
