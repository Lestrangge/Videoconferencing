using System.Collections.Generic;
using System.Threading.Tasks;
using VideoconferencingBackend.Models.DBModels;

namespace VideoconferencingBackend.Interfaces.Repositories
{
    public interface IGroupsRepository : IRepository<Group>
    {
        Task<IEnumerable<Group>> GetCreatedGroups(string name, int page, int pageSize);
        Task<Group> CreateWithOwner(Group item, string ownerLogin);
        Task<Group> AddToGroup(string userLogin, string groupName);
        Task<IEnumerable<Group>> GetUsersGroups(string userLogin, int page, int pageSize);
        Task<IEnumerable<User>> GetGroupUsers(string groupName, int page, int pageSize);
    }   
}
