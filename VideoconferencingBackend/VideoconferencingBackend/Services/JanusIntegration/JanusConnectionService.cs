using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using VideoconferencingBackend.Interfaces.Adapters;
using VideoconferencingBackend.Interfaces.Services.Janus;
using VideoconferencingBackend.Models.Janus;
using VideoconferencingBackend.Models.Janus.JanusApi.JanusResponse;
using VideoconferencingBackend.Models.Janus.PluginApi.PluginResponse;
using VideoconferencingBackend.Utils;

namespace VideoconferencingBackend.Services.JanusIntegration
{
    public partial class JanusApiService : IJanusConnectionService
    {
        private readonly int _janusTimeout = int.MaxValue;

        private readonly Dictionary<string, JanusMessageEvent> _pendingRequests =
            new Dictionary<string, JanusMessageEvent>();

        private readonly IWebSocketAdapter _ws;

        public TResponseType Send<TResponseType>(JanusBase request, bool releaseOnAck = false)
            where TResponseType : JanusBase
        {
            _pendingRequests.Add(request.Transaction, new JanusMessageEvent(releaseOnAck));
            var payload = JsonConvert.SerializeObject(request, _snakeCase);
            var listener = new Task<string>(() => _pendingRequests[request.Transaction].WaitOne(_janusTimeout));
            listener.Start();
            Send(payload).Wait();
            var response = listener.Result;
            if (response == null)
                throw new TimeoutException("Janus timeout");
            if (JsonConvert.DeserializeObject<JanusBase>(response, _snakeCase) is JanusBase responseBase &&
                responseBase.Janus != "Error")
                return JsonConvert.DeserializeObject<TResponseType>(response, _snakeCase);

            if (JsonConvert.DeserializeObject<JanusErrorResponse>(response, _snakeCase) is JanusErrorResponse janusError
                && !string.IsNullOrEmpty(janusError.Error.Reason))
                throw new ArgumentException(janusError.Error.Reason);
            if (JsonConvert.DeserializeObject<PluginErrorResponse>(response, _snakeCase) is PluginErrorResponse pluginError
                && !string.IsNullOrEmpty(pluginError.Plugindata.Data.Error))
                throw new ArgumentException(pluginError.Plugindata.Data.Error);
            throw new FormatException("Unknown janus response");
        }

        private void MessagesHandler(string payload)
        {
            var baseResponse = JsonConvert.DeserializeObject<JanusBase>(payload, _snakeCase);
            var transaction = baseResponse.Transaction;
            if (baseResponse.Janus == "ack"
                && _pendingRequests.ContainsKey(transaction)
                && !_pendingRequests[transaction].ReleaseOnAck)
                return;
            if (!string.IsNullOrEmpty(transaction) && _pendingRequests.ContainsKey(transaction))
            {
                _pendingRequests[transaction].Set(payload);
                _pendingRequests.Remove(transaction);
            }
            else
            {
                MessageHandler(payload);
            }
        }

        private async Task Send(string payload)
        {
            if (!_ws.IsAlive)
                await _ws.Connect();
            await _ws.Send(payload);
        }
    }
}