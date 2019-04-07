using System.Threading.Tasks;
using VideoconferencingBackend.Interfaces.Services.Janus;

namespace VideoconferencingBackend.Services.JanusIntegration
{
    public class JanusApiMockService : IJanusApiService
    {
        public Task Register(string username)
        {
            return Task.CompletedTask;
        }

        public Task CreateRoom()
        {
            return Task.CompletedTask;
        }

        public Task JoinRoom()
        {
            return Task.CompletedTask;
        }
    }
}
