using NLog;
using System;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VideoconferencingBackend.Interfaces.Adapters;

namespace VideoconferencingBackend.Adapters
{
    public class WebSocketAdapter : IWebSocketAdapter
    {
        private ClientWebSocket _ws;
        private event Action<string> OnMessageReceived;
        private event Action<string> OnDisconnected;
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private readonly Uri _endpoint;
        public WebSocketAdapter(string endpoint)
        {
            _endpoint = new Uri(endpoint);
            _ws = new ClientWebSocket();
            _ws.Options.AddSubProtocol("janus-protocol");
            OnDisconnected += (reason) => { _logger.Trace($"[Native WebSocket client] Disconnected: {reason}"); };
        }

        public void Dispose()
        {
            _ws.Dispose();
        }

        public async Task Connect()
        {
            if (_ws.State != WebSocketState.Open)
            {
                _ws.Dispose();
                _ws = new ClientWebSocket();
                _ws.Options.AddSubProtocol("janus-protocol");
                OnMessageReceived += (message) => { _logger.Trace($"[Native WebSocket client] Message received: {message}"); };
                OnDisconnected += (reason) => { _logger.Trace($"[Native WebSocket client] Disconnected: {reason}"); };
                await _ws.ConnectAsync(_endpoint, CancellationToken.None);
                Listen(_ws);
            }
        }

        public void AddOnMessage(Action<string> messageListener)
        {
            OnMessageReceived += messageListener;
        }

        public bool IsAlive => _ws.State == WebSocketState.Open;

        public Task Send(string payload)
        {
            _logger.Trace($"[Native WebSocket client] Message sent: {payload}");
            return _ws.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(payload)), WebSocketMessageType.Text, true,
                CancellationToken.None);
        }

        public void AddOnDisconnected(Action<string> disconnectionListener)
        {
            OnDisconnected += disconnectionListener;
        }

        private async Task Listen(ClientWebSocket ws)
        {
            try
            {
                do
                {
                    var buffer = new ArraySegment<byte>(new byte[4096]);
                    using (var ms = new MemoryStream())
                    {
                        WebSocketReceiveResult received;
                        do
                        {
                            received = await _ws.ReceiveAsync(buffer, CancellationToken.None);
                            if (buffer.Array != null)
                                ms.Write(buffer.Array, buffer.Offset, received.Count);
                        } while (!received.EndOfMessage);

                        ms.Seek(0, SeekOrigin.Begin);
                        var message = Encoding.UTF8.GetString(ms.ToArray());
                        OnMessageReceived?.Invoke(message);
                    }
                } while (ws.State != WebSocketState.Closed);
            }
            catch (Exception ex)
            {
                OnDisconnected?.Invoke(ex.Message);
            }
        }
    }
}
