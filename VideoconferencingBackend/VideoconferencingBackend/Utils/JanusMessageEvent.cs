using System;
using System.Threading;

namespace VideoconferencingBackend.Utils
{
    public class JanusMessageEvent : IDisposable
    {
        private readonly AutoResetEvent _ev;
        private string _message = "";

        public JanusMessageEvent(bool releaseOnAck = false, bool initialState = false)
        {
            _ev = new AutoResetEvent(initialState);
            ReleaseOnAck = releaseOnAck;
        }

        public bool ReleaseOnAck { get; }

        public void Dispose()
        {
            _ev?.Dispose();
        }

        public void Set(string message)
        {
            _message = message;
            _ev.Set();
        }

        public string WaitOne()
        {
            _ev.WaitOne();
            return _message;
        }

        public string WaitOne(int millisecondsTimeout)
        {
            return _ev.WaitOne(millisecondsTimeout) ? _message : null;
        }
    }
}