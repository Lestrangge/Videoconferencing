namespace VideoconferencingBackend.Interfaces.Services.Janus
{
    public interface IJanusMessagesHandlerService
    {
        void OnDisconnected(string reason);
        void MessageHandler(string payload);
    }
}
