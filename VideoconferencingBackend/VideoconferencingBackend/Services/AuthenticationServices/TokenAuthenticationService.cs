using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using VideoconferencingBackend.Interfaces.Repositories;
using VideoconferencingBackend.Interfaces.Services.Authentication;
using VideoconferencingBackend.Models.DBModels;

namespace VideoconferencingBackend.Services.AuthenticationServices
{
    public class TokenAuthenticationService : ICustomAuthenticationService
    {
        private readonly IUsersRepository _users;
        private readonly IHasherService _hasher;
        private readonly IConfiguration _config;
        private readonly ITokenGeneratorService _tokenGenerator;

        public TokenAuthenticationService(IUsersRepository users, IHasherService hasher, 
            IConfiguration config, ITokenGeneratorService tokenGenerator)
        {
            _users = users;
            _hasher = hasher;
            _config = config;
            _tokenGenerator = tokenGenerator;
        }
        public async Task<string> Signup(User credentials)
        {
            var user = await _users.Get(credentials.Login);
            if (user != null)
                throw new ArgumentException("Login already taken");
            if (Zxcvbn.Zxcvbn.MatchPassword(credentials.Password).Score < int.Parse(_config["PasswordEnthropy"]))
                throw new ArgumentException("Password is too weak");
            credentials.Password = _hasher.Hash(credentials.Password);
            var authorized = await _users.Create(credentials);
            var token = _tokenGenerator.GenerateToken(authorized);

            return token;
        }

        public async Task<string> Login(User credentials)
        {
            var user = await _users.Get(credentials.Login);
            if(user == null)
                throw new ArgumentException("User not found");
            if (user.Password != _hasher.Hash(credentials.Password))
                throw new ArgumentException("Password is wrong");
            return _tokenGenerator.GenerateToken(user);
        }
    }
}

