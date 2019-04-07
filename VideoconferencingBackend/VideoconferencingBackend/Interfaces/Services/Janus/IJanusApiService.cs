using System.Threading.Tasks;

namespace VideoconferencingBackend.Interfaces.Services.Janus
{
    public interface IJanusApiService
    {
        Task Register(string username);
        Task CreateRoom();
        Task JoinRoom();

    }
}
