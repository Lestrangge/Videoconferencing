namespace VideoconferencingBackend.Models.Janus.PluginApi.PluginRequest
{
    public class JoinRequest : PluginRequestBase<JoinRequestBody> { }

    public class JoinRequestBody : PluginRequestBodyBase
    {
        public override string Request => "join";
        public string Ptype => "subscriber";
        public long Feed { get; set; }
    }
}
