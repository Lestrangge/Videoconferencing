using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using VideoconferencingBackend.Interfaces.Repositories;
using VideoconferencingBackend.Interfaces.Services.Authentication;
using VideoconferencingBackend.Models.DBModels;

namespace VideoconferencingBackend.Services.AuthenticationServices
{
    public class CookiesAuthenticationService : ICustomAuthenticationService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IHasherService _hasher;
        private readonly IHttpContextAccessor _accessor;

        public CookiesAuthenticationService(IUsersRepository usersRepository, IHasherService hasher, IHttpContextAccessor accessor)
        {
            _hasher = hasher;
            _usersRepository = usersRepository;
            _accessor = accessor;
        }

        public async Task<string> Signup(User credentials)
        {
            if (await _usersRepository.GetByLogin(credentials.Login) != null)
                throw new ArgumentException("Login is already taken");
            if (Zxcvbn.Zxcvbn.MatchPassword(credentials.Password).Score < 2)
                throw new ArgumentException("Password is weak");

            credentials.Password = _hasher.Hash(credentials.Password);
            await _usersRepository.Create(credentials);
            await SetCookies(credentials);

            return "";
        }

        public async Task<string> Login(User credentials)
        {
            var user = await _usersRepository.GetByLogin(credentials.Login);
            if (user == null)
                throw new ArgumentException("User not found");
            if(user.Password != _hasher.Hash(credentials.Password))
                throw new ArgumentException("Wrong password");
            await SetCookies(user);
            return "";
        }

        private Task SetCookies(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.UserGuid),
            };
            var claimsIdentity = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            return _accessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));
        }
    }
}
