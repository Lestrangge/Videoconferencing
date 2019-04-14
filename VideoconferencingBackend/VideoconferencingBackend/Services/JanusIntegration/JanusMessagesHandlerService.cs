using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Linq;
using System.Threading.Tasks;
using VideoconferencingBackend.DTO.Hub.ServerEvents;
using VideoconferencingBackend.Hubs;
using VideoconferencingBackend.Interfaces.Repositories;
using VideoconferencingBackend.Models.DBModels;
using VideoconferencingBackend.Models.Janus;
using VideoconferencingBackend.Models.Janus.PluginApi.PluginResponse;
using VideoconferencingBackend.Utils;

namespace VideoconferencingBackend.Services.JanusIntegration
{
    public partial class JanusApiService
    {
        /// <summary>
        ///     Serializer settings to work correctly with snake_case
        /// </summary>
        private readonly JsonSerializerSettings _snakeCase = new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            },
            Formatting = Formatting.Indented
        };

        public void OnDisconnected(string reason)
        {
            throw new System.NotImplementedException();
        }

        public void MessageHandler(string payload)
        {
            var a = "";
            var baseJanusResponse = JsonConvert.DeserializeObject<PluginResponseBase>(payload, _snakeCase);
            switch (baseJanusResponse.Janus)
            {
                case "webrtcup":
                {
                    WebRTCUpHandler(JsonConvert.DeserializeObject<WebRtcUpResponse>(payload, _snakeCase)).Wait();
                    return;
                }
                case "media":
                {
                    MediaHandler(JsonConvert.DeserializeObject<MediaResponse>(payload, _snakeCase)).Wait();
                    return;
                }
                default:
                {
                    if (baseJanusResponse.Janus == "event")
                    {
                        var newPublisher = JsonConvert.DeserializeObject<NewAvailablePublisherResponse>(payload, _snakeCase);
                        var unpublished = JsonConvert.DeserializeObject<UnpublishedResponse>(payload, _snakeCase);
                        if (!newPublisher.Plugindata.Data.Publishers.IsNullOrEmpty())
                        {
                            NewAvailablePublisherHandler(newPublisher).Wait();
                            return;
                        }
                        else if (unpublished.Plugindata.Data.Unpublished != null)
                        {
                            UnpublishedHandler(unpublished).Wait();
                        }
                        var basePluginResponse = JsonConvert.DeserializeObject<PluginResponseBase>(payload, _snakeCase);
                        

                    }
                    return;
                }
            }

        }
        private async Task HandleMessage(JanusBase response, string clientMethod, params object[] parameters)
        {
            User user;
            using (var scope = _scopeFactory.CreateScope())
            {
                user = await scope.ServiceProvider.GetService<IUsersRepository>().GetBySessionId(response.SessionId);
            }
            _logger.Trace(
                $"Janus response {clientMethod}: {JsonConvert.SerializeObject(response, _snakeCase)}\r\nSent to {JsonConvert.SerializeObject(user)}");
            await _hub.Clients.Client(user.ConnectionId).SendAsync(clientMethod, parameters);
        }

        private Task WebRTCUpHandler(WebRtcUpResponse response)
        {
            return HandleMessage(response, "WebRTCUp");
        }

        private Task MediaHandler(MediaResponse response)
        {
            return HandleMessage(response, "Media", new MediaEvent(response));
        }

        private async Task NewAvailablePublisherHandler(NewAvailablePublisherResponse response)
        {
            User user;
            using (var scope = _scopeFactory.CreateScope())
            {
                user = await scope.ServiceProvider.GetService<IUsersRepository>().GetBySessionId(response.SessionId);
                if (user == null)
                    return;
            }
            user.HandleId = await AttachPlugin(user);
            var connection = user.ConnectionId;
            var feed = response.Plugindata.Data.Publishers?.FirstOrDefault()?.Id;
            var offer = await JoinPublisher((long)feed, user);
            await _hub.Clients.Client(connection).SendAsync("NewPublisher", new NewPublisherEvent((long)user.HandleId, offer,user));
        }

        private async Task UnpublishedHandler(UnpublishedResponse response)
        {
            string connection;
            using (var scope = _scopeFactory.CreateScope())
            {
                connection = (await scope.ServiceProvider.GetService<IUsersRepository>().GetBySessionId(response.SessionId)).ConnectionId;
            }
            if(!string.IsNullOrEmpty(connection))
                await _hub.Clients.Client(connection).SendAsync("Unpublished", new UnpublishedEvent{HandleId = (long)response.Plugindata.Data.Unpublished });
        }
    }
}
