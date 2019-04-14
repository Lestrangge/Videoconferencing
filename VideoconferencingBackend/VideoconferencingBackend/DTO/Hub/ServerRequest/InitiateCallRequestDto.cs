using VideoconferencingBackend.Models.Janus.PluginApi;

namespace VideoconferencingBackend.DTO.Hub.ServerRequest
{
    public class InitiateCallRequestDto
    {
        public Jsep Offer { get; set; }
        public string GroupGuid { get; set; }   
    }
}
