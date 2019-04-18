using System.Threading.Tasks;
using VideoconferencingBackend.Models.DBModels;

namespace VideoconferencingBackend.Interfaces.Repositories
{
    public interface IUsersRepository : IRepository<User>
    {
        Task<User> GetByLogin(string login);
        Task<User> GetBySessionId(long? responseSessionId);
        Task<User> GetByHandleId(long? handleId);
        Task<User> UpdateInCall(User item, Group groupInCall);
    }
}
