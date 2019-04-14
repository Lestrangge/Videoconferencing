using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NLog;
using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using VideoconferencingBackend.Hubs;
using VideoconferencingBackend.Interfaces.Adapters;
using VideoconferencingBackend.Interfaces.Repositories;
using VideoconferencingBackend.Interfaces.Services.Janus;
using VideoconferencingBackend.Models.DBModels;
using VideoconferencingBackend.Models.Janus;
using VideoconferencingBackend.Models.Janus.JanusApi.JanusRequests;
using VideoconferencingBackend.Models.Janus.JanusApi.JanusResponse;
using VideoconferencingBackend.Models.Janus.PluginApi;
using VideoconferencingBackend.Models.Janus.PluginApi.PluginRequest;
using VideoconferencingBackend.Models.Janus.PluginApi.PluginResponse;

namespace VideoconferencingBackend.Services.JanusIntegration
{
    public partial class JanusApiService : IJanusApiService
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly Random _random = new Random();
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private readonly IServiceProvider _scopeFactory;
        private readonly IHubContext<JanusMessagesHub> _hub;


        public JanusApiService(IHttpContextAccessor accessor,IServiceProvider scopeFactory, IWebSocketAdapter ws,
            IConfiguration config, IHubContext<JanusMessagesHub> hub)
        {
            _accessor = accessor;
            _scopeFactory = scopeFactory;
            _hub = hub;
            _ws = ws;
            _ws.AddOnDisconnected(OnDisconnected);
            _ws.AddOnMessage(MessagesHandler);
            int.TryParse(config["JanusTimeout"], out var timeout);
            if (timeout > 0)
                _janusTimeout = timeout;
        }

        #region prepareUser
        private async Task<long?> CreateSession()
        {
            var request = new CreateSessionRequest(RandomString());
            _logger.Trace($"Janus create session request: {JsonConvert.SerializeObject(request)}");
            return (Send<SuccessJanus>(request)).Data.Id;
        }

        public async Task<long?> AttachPlugin(User one = null)
        {
            var request = new AttachPluginRequest();
            _logger.Trace($"Janus attach plugin request: {JsonConvert.SerializeObject(request)}");
            return (await SendJanusRequest<SuccessJanus>(request, one: one)).Data.Id;
        }

        private async Task<User> PrepareUser(User user)
        {
            user.SessionId = await CreateSession();
            using (var scope = _scopeFactory.CreateScope())
            {
                await scope.ServiceProvider.GetService<IUsersRepository>().Update(user);
            }
            user.HandleId = await AttachPlugin();
            using (var scope = _scopeFactory.CreateScope())
            {
                await scope.ServiceProvider.GetService<IUsersRepository>().Update(user);
            }
            return user;
        }
        #endregion

        public async Task<Jsep> InitiateCall(string groupGuid, Jsep jsep)
        {
            var user = await PrepareUser(await MakeUser());
            var request = new JoinAndConfigureRequest
            {
                Transaction = RandomString(),
                Body = new JoinAndConfigureRequestBody
                {
                    Audio = true,
                    Video = true,
                    Room = 1234,
                    Display = user.Login
                },
                Jsep = jsep

            };
            var result = await SendPluginRequest<JoinAndConfigureRequestBody, JoinAndConfigureResponse>(request, false);
            var publishers = result.Plugindata.Data.Publishers;
            foreach (var publisher in publishers)
            {
#pragma warning disable 4014
                NewAvailablePublisherHandler(new NewAvailablePublisherResponse
#pragma warning restore 4014
                {
                    SessionId = user.SessionId,
                    Plugindata = new NewAvailablePublisherResponsePluginData
                    {
                        Data = new NewAvailablePublisherResponseData
                        {
                            Publishers = new []{publisher}
                        }
                    }
                });
            }
            return result.Jsep;
        }

        public async Task<string> Trickle(TrickleCandidateReceived candidateReceived)
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
            request.HandleId = (await MakeUser()).HandleId;
            _logger.Trace($"Janus Trickle request: {JsonConvert.SerializeObject(request)}");

            var res = await SendJanusRequest<AckResponse>(request, true);
            return res.Janus;
        }

        public async Task<Jsep> JoinPublisher(long feed, User one)
        {
            var request = new JoinRequest
            {
                HandleId = one.HandleId,
                Body = new JoinRequestBody
                {
                    Feed = feed,
                    Room = 1234
                }
            };
            var result = await SendJanusRequest<JoinResponse>(request, false, one:one);
            return result.Jsep;
        }

        public async Task<string> StartPeerConnection(Jsep answer, long handleId)
        {
            var request = new StartRequest()
            {
                Jsep = answer,
                HandleId = handleId,
                Body = new StartRequestBody
                {
                    Room = 1234
                }
            };
            var result = await SendJanusRequest<StartResponse>(request, false);
            return result.Plugindata.Data.Started;
        }

        public async Task<string> Destroy()
        {
            var request = new DestroyRequest();
            var result = await SendJanusRequest<SuccessJanus>(request, false);
            return result.Janus;
        }

        #region Senders
        private async Task<TR> SendPluginRequest<TB, TR>(PluginRequestBase<TB> message, bool release = false, User one = null)
            where TB : PluginRequestBodyBase
            where TR : JanusBase
        {
            var user = one ?? await MakeUser();
            message.SessionId = user.SessionId;
            message.Transaction = RandomString();
            message.HandleId = user.HandleId;
            return await Task.Run(() => Send<TR>(message, release));
        }
        private async Task<TR> SendJanusRequest<TR>(JanusBase message, bool release = false, User one = null) where TR : JanusBase
        {
            var user = one ?? await MakeUser();
            message.SessionId = user.SessionId;
            message.Transaction = RandomString();
            return await Task.Run(() => Send<TR>(message, release));
        }
        #endregion

        #region Helpers
        private async Task<User> MakeUser()
        {
            User user;
            using (var scope = _scopeFactory.CreateScope())
            {
                user = await scope.ServiceProvider.GetService<IUsersRepository>().Get(_accessor.HttpContext.User.Identity.Name);
            }

            return user;
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
        #endregion

    }
}
