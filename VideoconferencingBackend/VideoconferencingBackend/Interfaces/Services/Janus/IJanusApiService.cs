using System.Threading.Tasks;
using VideoconferencingBackend.Models.Janus.PluginApi;

namespace VideoconferencingBackend.Interfaces.Services.Janus
{
    public interface IJanusApiService
    {
        Task Register(string username);
        Task CreateRoom();
        Task JoinRoom();
        Task<Jsep> InitiateCall(string groupName);
        Task StartCall(Jsep jsep);
    }
}
