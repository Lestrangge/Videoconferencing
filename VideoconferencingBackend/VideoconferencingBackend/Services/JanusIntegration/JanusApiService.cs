using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VideoconferencingBackend.Interfaces.Services.Janus;
using VideoconferencingBackend.Models.Janus.PluginApi;
using VideoconferencingBackend.Models.Janus.PluginApi.PluginRequest;

namespace VideoconferencingBackend.Services.JanusIntegration
{
    public class JanusApiService : IJanusApiService
    {

        public Task Register(string username)
        {
            throw new NotImplementedException();
        }

        public Task CreateRoom()
        {
            throw new NotImplementedException();
        }

        public Task JoinRoom()
        {
            throw new NotImplementedException();
        }

        public Task<Jsep> InitiateCall(string groupName)
        {
            throw new NotImplementedException();
        }

        public Task StartCall(Jsep jsep)
        {
            throw new NotImplementedException();
        }

        //public Task<Jsep> InitiateCall(string groupName, Jsep jsep)
        //{
        //    return Task.FromResult(new Jsep{Sdp = "", Type = "answer"});
        //}

        //public Task<ClientResponse> Trickle(TrickleCandidateReceived candidateReceived)
        //{
        //    var request = new TrickleRequest();
        //    if (candidateReceived.Completed)
        //        request.Candidate = new TrickleCompleted { Completed = true };
        //    else
        //        request.Candidate = new TrickleCandidate
        //        {
        //            Candidate = candidateReceived.Candidate,
        //            SdpMLineIndex = candidateReceived.SdpMLineIndex,
        //            SdpMid = candidateReceived.SdpMid
        //        };
        //    _logger.Trace($"Janus Trickle request: {JsonConvert.SerializeObject(request)}");
        //    return SendPluginRequest(request, true);
        //}
    }
}
