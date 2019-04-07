using System;
using System.Threading.Tasks;

namespace VideoconferencingBackend.Interfaces.Adapters
{
    public interface IWebSocketAdapter : IDisposable
    {
        /// <summary>
        /// Sends a message via websocket interface asynchronously
        /// </summary>
        /// <param name="data">Message to be sent</param>
        /// <returns></returns>
        Task SendAsync(string data);
        /// <summary>
        /// Sends a message via websocket interface synchronously
        /// </summary>
        /// <param name="data">Message to be sent</param>
        /// <returns></returns>
        void Send(string data);
        
    }
}
