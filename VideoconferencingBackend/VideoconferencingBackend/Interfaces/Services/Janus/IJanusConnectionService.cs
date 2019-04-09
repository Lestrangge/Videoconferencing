using System.Threading.Tasks;
using VideoconferencingBackend.Models.Janus;

namespace VideoconferencingBackend.Interfaces.Services.Janus
{
    public interface IJanusConnectionService
    {
        TResponseType Send<TResponseType>(JanusBase request, bool releaseOnAck = false) where TResponseType : JanusBase;
    }
}
