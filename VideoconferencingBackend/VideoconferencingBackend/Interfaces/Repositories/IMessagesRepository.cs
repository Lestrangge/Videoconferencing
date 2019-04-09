using System.Collections.Generic;
using System.Threading.Tasks;
using VideoconferencingBackend.Models.DBModels;

namespace VideoconferencingBackend.Interfaces.Repositories
{
    public interface IMessagesRepository : IRepository<Message>
    {
        Task<IEnumerable<Message>> GetMessagesFromGroup(string groupName, int? page = null, int? pageSize = null);
    }
}
