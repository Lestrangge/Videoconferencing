using System.Threading.Tasks;
using VideoconferencingBackend.Models.DBModels;

namespace VideoconferencingBackend.Interfaces.Repositories
{
    public interface IUsersRepository : IRepository<User>
    {
        Task<User> GetByLogin(string login);
    }
}
