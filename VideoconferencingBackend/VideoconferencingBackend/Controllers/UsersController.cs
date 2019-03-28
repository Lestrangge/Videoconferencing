using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VideoconferencingBackend.DTO;
using VideoconferencingBackend.Interfaces;
using VideoconferencingBackend.Models.DBModels;

namespace VideoconferencingBackend.Controllers
{
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly IRepository<User> _usersRepository;
        private readonly IHasherService _hasher;
        private readonly ICustomAuthenticationService _authentication;
        public UsersController(IRepository<User> users, IHasherService hasher, ICustomAuthenticationService authentication)
        {
            _usersRepository = users;
            _hasher = hasher;
            _authentication = authentication;
        }

        /// <summary>
        /// Creates new user
        /// </summary>
        /// <param name="credentials">User to create</param>
        /// <returns>Jwt token</returns>
        /// <response code="200">Returns JWT token</response>
        /// <response code="400">If the login is taken or password is weak</response>    
        [HttpPost]
        [Route("signup")]
        public async Task<IActionResult> Signup([FromBody]UserSignupDTO credentials) 
        {
            if(!ModelState.IsValid) 
                return new BadRequestObjectResult(ModelState.Values.Select(value => value.Errors.FirstOrDefault()).FirstOrDefault().ErrorMessage);
            return await _authentication.Signup((User)credentials);
        }
            
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody]UserLoginDTO credentials)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult(ModelState.Values.Select(value => value.Errors.FirstOrDefault()).FirstOrDefault().ErrorMessage);
            return await _authentication.Login((User)credentials);
        }
    }
}