using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VideoconferencingBackend.Interfaces.Services
{
    public interface ISignalRConnectionService
    {
        Task OnConnected(string username);
        Task OnDisconected(string username);
    }
}
