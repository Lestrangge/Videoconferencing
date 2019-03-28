using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VideoconferencingBackend.Interfaces;
using VideoconferencingBackend.Models.DBModels;

namespace VideoconferencingBackend.Services
{
    public class CookiesAuthenticationService : ICustomAuthenticationService
    {
        private readonly IRepository<User> _usersRepository;
        private readonly IHasherService _hasher;
        private readonly IHttpContextAccessor _accessor;

        public CookiesAuthenticationService(IRepository<User> usersRepository, IHasherService hasher, IHttpContextAccessor accessor)
        {
            _hasher = hasher;
            _usersRepository = usersRepository;
            _accessor = accessor;
        }

        public async Task<IActionResult> Signup(User credentials)
        {
            if (await _usersRepository.Get(credentials.Login) != null)
                return new BadRequestObjectResult("Login is already taken");
            if (Zxcvbn.Zxcvbn.MatchPassword(credentials.Password).Score < 2)
                return new BadRequestObjectResult("Paasword is weak");

            credentials.Password = _hasher.Hash(credentials.Password);
            await _usersRepository.Create(credentials);
            await SetCookies(credentials);

            return new OkResult();
        }

        public async Task<IActionResult> Login(User credentials)
        {
            var user = await _usersRepository.Get(credentials.Login);
            if (user == null)
                return new BadRequestObjectResult("User not found");
            if(user.Password != _hasher.Hash(credentials.Password))
                return new BadRequestObjectResult("Wrong password");
            await SetCookies(user);
            return new OkResult();
        }

        private Task SetCookies(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.Name),
            };
            var claimsIdentity = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            return _accessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));
        }
    }
}
