using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using VideoconferencingBackend.DTO.User.Requests;
using VideoconferencingBackend.DTO.User.Responses;
using VideoconferencingBackend.Interfaces.Repositories;
using VideoconferencingBackend.Interfaces.Services.Authentication;
using VideoconferencingBackend.Models.DBModels;

namespace VideoconferencingBackend.Controllers
{
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly IUsersRepository _usersRepository;
        private readonly ICustomAuthenticationService _authentication;
        private readonly int _pageSize;
        public UsersController(IUsersRepository users, ICustomAuthenticationService authentication, IConfiguration config)
        {
            _usersRepository = users;
            _authentication = authentication;
            _pageSize = int.Parse(config["UsersFindPageSize"]);
        }

        /// <summary>
        /// Creates new user
        /// </summary>
        /// <param name="credentials">User to create</param>
        /// <returns>Jwt token</returns>
        /// <response code="200">JWT token</response>
        /// <response code="400">If the login is taken or password is weak</response>    
        [HttpPost]
        [Route("signup")]
        public async Task<IActionResult> Signup([FromBody]UserSignupDto credentials) 
        {
            if(!ModelState.IsValid) 
                return new BadRequestObjectResult(ModelState.Values.Select(value => value.Errors.FirstOrDefault()).FirstOrDefault()?.ErrorMessage);
            try
            {
                return new OkObjectResult(await _authentication.Signup((User) credentials));
            }
            catch (ArgumentException ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        /// <summary>
        /// Authorize user as existing one
        /// </summary>
        /// <param name="credentials">User data to authorize</param>
        /// <returns>Jwt token</returns>
        /// <response code="200">Returns JWT token</response>
        /// <response code="400">If the login is not found or password is wrong</response>  
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody]UserLoginDto credentials)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult(ModelState.Values.Select(value => value.Errors.FirstOrDefault()).FirstOrDefault()?.ErrorMessage);
            try
            {
                return new OkObjectResult(await _authentication.Login((User) credentials));
            }
            catch (ArgumentException ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        /// <summary>
        /// TEST: returns users login
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("get_username")]
        public string Username()
        {
            return $"{HttpContext.User.Identity.Name}";
        }

        /// <summary>
        /// Returns enumerable of users, whose login contains pattern
        /// </summary>
        /// <param name="pattern">pattern contained in login</param>
        /// <param name="page">optional: page number (base 0)</param>
        /// <param name="pageSize">optional: page size (base 10)</param>
        /// <returns>Enumerable of found users</returns>
        /// <response code="200">Enumerable of found users</response>
        /// <response code="401">Unauthorized</response>  
        [HttpGet]
        [Authorize]
        [Route("find")]
        public async Task<IActionResult> Find(string pattern, int? pageSize, int? page)
        {
            return new OkObjectResult(new {Users = (await _usersRepository.Find(pattern, page ?? 0, pageSize ?? _pageSize))
                .Select(user => new UserFoundDto(user))});  
        }

    }
}