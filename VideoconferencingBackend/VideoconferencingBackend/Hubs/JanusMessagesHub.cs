using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using NLog;
using VideoconferencingBackend.Interfaces.Services.Janus;

namespace VideoconferencingBackend.Hubs
{
    public class JanusMessagesHub : Hub
    {
        private readonly IJanusApiService _janus;
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        public JanusMessagesHub(IJanusApiService janus)
        {
            _janus = janus;
        }

        public string TestFree()
        {
            return $"Ur cool, {Context.User.Identity.Name}, u made it! Now invoke Test to check whether u can access auth-only requests or not";
        }

        [Authorize]
        public string Test()
        {
            return "Ur cool, u got it, " + Context.User.Identity.Name;
        }
    }
}
