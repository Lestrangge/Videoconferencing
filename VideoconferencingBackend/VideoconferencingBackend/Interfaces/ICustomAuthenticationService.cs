using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VideoconferencingBackend.Models.DBModels;

namespace VideoconferencingBackend.Interfaces
{
    public interface ICustomAuthenticationService
    {   
        Task<IActionResult> Signup(User credentials);
        Task<IActionResult> Login(User credentials);
    }
}
