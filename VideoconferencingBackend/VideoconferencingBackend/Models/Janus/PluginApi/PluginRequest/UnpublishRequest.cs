namespace VideoconferencingBackend.Models.Janus.PluginApi.PluginRequest
{
    public class UnpublishRequest : PluginRequestBase<UnbublishRequestBody>
    {

    }

    public class UnbublishRequestBody : PluginRequestBodyBase
    {
        public override string Request => "unpublish";
    }
}
