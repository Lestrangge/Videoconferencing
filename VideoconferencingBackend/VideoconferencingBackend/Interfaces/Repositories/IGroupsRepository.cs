using System.Collections.Generic;
using System.Threading.Tasks;
using VideoconferencingBackend.Models.DBModels;

namespace VideoconferencingBackend.Interfaces.Repositories
{
    public interface IGroupsRepository : IRepository<Group>
    {
        Task<IEnumerable<Group>> GetCreatedGroups(string userGuid, int? page = null, int? pageSize = null);
        Task<Group> CreateWithOwner(Group item, string userGuid);
        Task<Group> AddToGroup(string userGuid, string groupGuid);
        Task<IEnumerable<Group>> GetUsersGroups(string userGuid, int? page = null, int? pageSize = null);
        Task<int> GetUsersGroupsLength(string userGuid);
        Task<IEnumerable<User>> GetGroupUsers(string groupGuid, int? page = null, int? pageSize = null);
        Task<Group> GetByName(string name);
        Task<Group> UpdateInCall(Group item, bool inCall);
    }   
}
