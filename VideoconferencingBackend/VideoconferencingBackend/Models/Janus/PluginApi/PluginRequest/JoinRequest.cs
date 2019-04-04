namespace VideoconferencingBackend.Models.Janus.PluginApi.PluginRequest
{
    public class JoinRequest : PluginBase<JoinBody>
    {
    }

    public class JoinBody : PluginBodyBase
    {
        public override string Request => "join";
        public string Ptype => "publisher";
        public string Display { get; set; }
    }
}
