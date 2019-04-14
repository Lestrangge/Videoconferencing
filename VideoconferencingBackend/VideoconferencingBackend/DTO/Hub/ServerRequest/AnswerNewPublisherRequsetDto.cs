using VideoconferencingBackend.Models.Janus.PluginApi;

namespace VideoconferencingBackend.DTO.Hub.ServerRequest
{
    public class AnswerNewPublisherRequsetDto
    {
        public Jsep Answer { get; set; }
        public long HandleId { get; set; }
    }
}
