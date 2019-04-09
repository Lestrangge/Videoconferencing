using VideoconferencingBackend.Interfaces.Services.Janus;

namespace VideoconferencingBackend.Services.JanusIntegration
{
    public class JanusMessagesHandlerService : IJanusMessagesHandlerService
    {
        public void OnDisconnected(string reason)
        {
            throw new System.NotImplementedException();
        }

        public void MessageHandler(string payload)
        {
            throw new System.NotImplementedException();
        }
    }
}
