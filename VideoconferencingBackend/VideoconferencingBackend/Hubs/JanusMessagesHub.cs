using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace VideoconferencingBackend.Hubs
{
    [Authorize]
    public class JanusMessagesHub : Hub
    {
        public JanusMessagesHub()
        {

        }
    }
}
