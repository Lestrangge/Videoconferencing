namespace VideoconferencingBackend.Models.Janus.PluginApi.PluginRequest
{
    public class EditRequest : PluginRequestBase<EditVideoroomRequestBody>
    {
        public EditVideoroomRequestBody RequestBody { get; set; }
    }

    public class EditVideoroomRequestBody : PluginRequestBodyBase
    {
        public override string Request => "edit";
        public string NewDescription { get; set; }
        public string NewPin { get; set; }
        public bool NewIsPrivate { get; set; }
        public bool NewPublishers { get; set; }
        public bool Permanent { get; set; }
        public string Secret { get; set; }
    }
}
