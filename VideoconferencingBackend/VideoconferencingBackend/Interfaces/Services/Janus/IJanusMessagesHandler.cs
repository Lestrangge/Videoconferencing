namespace VideoconferencingBackend.Interfaces.Services.Janus
{
    public interface IJanusMessagesHandler
    {
        void OnDisconnected(string reason);
        void MessageHandler(string payload);
    }
}
