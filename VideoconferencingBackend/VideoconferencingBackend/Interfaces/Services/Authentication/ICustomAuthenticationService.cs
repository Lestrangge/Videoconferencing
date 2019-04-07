using System.Threading.Tasks;
using VideoconferencingBackend.Models.DBModels;

namespace VideoconferencingBackend.Interfaces.Services.Authentication
{
    public interface ICustomAuthenticationService
    {   
        Task<string> Signup(User credentials);
        Task<string> Login(User credentials);
    }
}
