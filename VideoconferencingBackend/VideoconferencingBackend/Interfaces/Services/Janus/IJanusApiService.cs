using System.Threading.Tasks;
using VideoconferencingBackend.Models.Janus.PluginApi;
using VideoconferencingBackend.Models.Janus.PluginApi.PluginRequest;

namespace VideoconferencingBackend.Interfaces.Services.Janus
{
    public interface IJanusApiService : IJanusMessagesHandler, IJanusConnectionService
    {
        Task<string> Trickle(TrickleCandidateReceivedDto candidateReceived);
        Task<Jsep> InitiateCall(string groupGuid,Jsep jsep);
        Task<long?> AttachPlugin();

        Task<Jsep> JoinPublisher(long feed, long handleId);
        Task<string> StartPeerConnection(Jsep answer, long handleId);


    }
}
