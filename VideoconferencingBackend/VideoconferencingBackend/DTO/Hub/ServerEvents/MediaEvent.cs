using VideoconferencingBackend.Models.Janus.PluginApi.PluginResponse;

namespace VideoconferencingBackend.DTO.Hub.ServerEvents
{
    public class MediaEvent
    {
        public string Type { get; set; }
        public bool Receiving { get; set; }

        public MediaEvent(MediaResponse response)
        {
            Type = response.Type;
            Receiving = response.Receiving;
        }
    }
}
