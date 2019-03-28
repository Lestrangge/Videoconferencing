using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using VideoconferencingBackend.Interfaces;
using VideoconferencingBackend.Models.DBModels;

namespace VideoconferencingBackend.Services
{
    public class TokenAuthenticationService : ICustomAuthenticationService
    {
        private readonly IRepository<User> _users;
        private readonly IHasherService _hasher;
        private readonly IConfiguration _config;
        private readonly ITokenGeneratorService _tokenGenerator;

        public TokenAuthenticationService(IRepository<User> users, IHasherService hasher, 
            IConfiguration config, ITokenGeneratorService tokenGenerator)
        {
            _users = users;
            _hasher = hasher;
            _config = config;
            _tokenGenerator = tokenGenerator;
        }
        public async Task<IActionResult> Signup(User credentials)
        {
            User user = await _users.Get(credentials.Login);
            if (user != null)
                return new BadRequestObjectResult("Login already taken");
            if (Zxcvbn.Zxcvbn.MatchPassword(credentials.Password).Score < int.Parse(_config["PasswordEnthropy"]))
                return new BadRequestObjectResult("Paasword is weak");
            var authorized = await _users.Create(credentials);
            var token = _tokenGenerator.GenerateToken(authorized);

            return new OkObjectResult(token);
        }

        public async Task<IActionResult> Login(User credentials)
        {
            User user = await _users.Get(credentials.Login);
            if(user == null)
                return new BadRequestObjectResult("User not found");
            if (user.Password != _hasher.Hash(credentials.Password))
                return new BadRequestObjectResult("Password is wrong");
            return new OkObjectResult(_tokenGenerator.GenerateToken(user));
        }


    }
}

