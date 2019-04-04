namespace VideoconferencingBackend.Models.Janus.PluginApi.PluginRequest
{
    public class DestroyRequest : PluginBase<DestroyVideoroomBody>
    {
        public override DestroyVideoroomBody Body { get; set; }
    }

    public class DestroyVideoroomBody : PluginBodyBase
    {
        public override string Request => "join";
        public bool Permanent { get; set; }
        public string Secret { get; set; }

    }
}
