using System.Threading.Tasks;
using VideoconferencingBackend.Models.DBModels;

namespace VideoconferencingBackend.Interfaces
{
    public interface ITokenGeneratorService
    {
        string GenerateToken(User user);
    }
}
