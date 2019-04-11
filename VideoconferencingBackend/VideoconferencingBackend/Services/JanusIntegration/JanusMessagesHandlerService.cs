using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NLog;
using VideoconferencingBackend.Hubs;
using VideoconferencingBackend.Interfaces.Repositories;
using VideoconferencingBackend.Interfaces.Services.Janus;
using VideoconferencingBackend.Models.DBModels;
using VideoconferencingBackend.Models.Janus;

namespace VideoconferencingBackend.Services.JanusIntegration
{
    public class JanusMessagesHandlerService : IJanusMessagesHandlerService
    {
        private readonly IHubContext<JanusMessagesHub> _hub;
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private readonly IServiceProvider _contextFactory;

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

        public JanusMessagesHandlerService(IServiceProvider contextFactory, IHubContext<JanusMessagesHub> hub)
        {
            _contextFactory = contextFactory;
            _hub = hub;
        }

        public void OnDisconnected(string reason)
        {
            throw new System.NotImplementedException();
        }

        public void MessageHandler(string payload)
        {
            throw new System.NotImplementedException();
        }
        private async Task HandleMessage(JanusBase response, string clientMethod, params object[] parameters)
        {
            User user;
            using (var context = _contextFactory.CreateScope())
            {
                var users = context.ServiceProvider.GetService<IUsersRepository>();
                user = await users.GetBySessionId(response.SessionId);
            }
            _logger.Trace(
                $"Janus response {clientMethod}: {JsonConvert.SerializeObject(response)}\r\nSent to {JsonConvert.SerializeObject(user)}");
            await _hub.Clients.Client(user.ConnectionId).SendAsync(clientMethod, parameters);
        }
    }
}
