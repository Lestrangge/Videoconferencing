using Microsoft.AspNetCore.Http;
using NLog;
using System;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VideoconferencingBackend.Interfaces.Repositories;
using VideoconferencingBackend.Interfaces.Services.Janus;
using VideoconferencingBackend.Models.DBModels;
using VideoconferencingBackend.Models.Janus;
using VideoconferencingBackend.Models.Janus.PluginApi;
using VideoconferencingBackend.Models.Janus.PluginApi.PluginRequest;
using VideoconferencingBackend.Models.Janus.PluginApi.PluginResponse;

namespace VideoconferencingBackend.Services.JanusIntegration
{
    public class JanusApiService : IJanusApiService
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly IJanusConnectionService _janus;
        private readonly Random _random = new Random();
        private readonly IUsersRepository _users;
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
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

        public Task<Jsep> InitiateCall(string groupName, Jsep jsep)
        {
            return Task.FromResult(new Jsep { Sdp = "", Type = "answer" });
        }

        public Task Trickle(TrickleCandidateReceived candidateReceived)
        {
            var request = new TrickleRequest();
            if (candidateReceived.Completed)
                request.Candidate = new TrickleCompleted { Completed = true };
            else
                request.Candidate = new TrickleCandidate
                {
                    Candidate = candidateReceived.Candidate,
                    SdpMLineIndex = candidateReceived.SdpMLineIndex,
                    SdpMid = candidateReceived.SdpMid
                };
            _logger.Trace($"Janus Trickle request: {JsonConvert.SerializeObject(request)}");
            return SendJanusRequest<AckResponse>(request, true);
        }

        private Task<User> MakeUser()
        {
            return _users.Get(_accessor.HttpContext.User.Identity.Name);
        }


        private string RandomString()
        {
            var builder = new StringBuilder();
            var size = _random.Next(10, 15);
            for (var i = 0; i < size; i++)
            {
                var ch = Convert.ToChar(_random.Next(26) + 65);
                builder.Append(ch);
            }
            return builder.ToString();
        }

        private async Task<TR> SendPluginRequest<TB, TR>(PluginBase<TB> message, bool release = false, User one = null) 
            where TB : PluginBodyBase
            where TR: JanusBase
        {
            var user = one ?? await MakeUser();
            message.SessionId = user.SessionId;
            message.Transaction = RandomString();
            message.HandleId = user.HandleId;
            return _janus.Send<TR>(message, release);
        }
        private async Task<TR> SendJanusRequest<TR>(JanusBase message, bool release = false, User one = null) where TR : JanusBase
        {
            var user = one ?? await MakeUser();
            message.SessionId = user.SessionId;
            message.Transaction = RandomString();
            return _janus.Send<TR>(message, release);
        }
    }
}
