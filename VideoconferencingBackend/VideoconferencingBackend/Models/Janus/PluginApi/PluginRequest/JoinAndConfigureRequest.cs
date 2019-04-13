namespace VideoconferencingBackend.Models.Janus.PluginApi.PluginRequest
{
    public class JoinAndConfigureRequest : PluginRequestBase<JoinAndConfigureRequestBody>
    {
        public Jsep Jsep { get; set; }

    }

    public class JoinAndConfigureRequestBody : PluginRequestBodyBase
    {
        public override string Request => "joinandconfigure";
        public string Ptype => "publisher";
        public string Display { get; set; }
        public bool Audio { get; set; }
        public bool Video { get; set; }
    }
}
