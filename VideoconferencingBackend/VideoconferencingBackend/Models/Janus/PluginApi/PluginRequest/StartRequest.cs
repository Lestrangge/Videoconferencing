namespace VideoconferencingBackend.Models.Janus.PluginApi.PluginRequest
{
    public class StartRequest : PluginRequestBase<StartRequestBody>
    {
        public Jsep Jsep { get; set; }
    }

    public class StartRequestBody : PluginRequestBodyBase
    {
        public override string Request => "start";
    }
}
