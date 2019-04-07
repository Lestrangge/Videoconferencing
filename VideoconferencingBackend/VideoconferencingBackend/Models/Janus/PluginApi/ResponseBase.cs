namespace VideoconferencingBackend.Models.Janus.PluginApi
{
    public class ResponseBase : JanusBase
    {
        public override string Janus
        {
            get { return "event"; }
        }

    }
}
