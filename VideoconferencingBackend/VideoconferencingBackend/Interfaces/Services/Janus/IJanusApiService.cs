using System.Threading.Tasks;
using VideoconferencingBackend.Models.DBModels;
using VideoconferencingBackend.Models.Janus.PluginApi;
using VideoconferencingBackend.Models.Janus.PluginApi.PluginRequest;

namespace VideoconferencingBackend.Interfaces.Services.Janus
{
    public interface IJanusApiService : IJanusMessagesHandler, IJanusConnectionService
    {
        Task<string> Trickle(TrickleCandidateReceived candidateReceived);
        Task<Jsep> InitiateCall(string groupGuid,Jsep jsep);
        Task<long?> AttachPlugin(User one = null);  

        Task<Jsep> JoinPublisher(long feed, User one);
        Task<string> StartPeerConnection(Jsep answer, long handleId);
        Task<string> Destroy();

    }
}
