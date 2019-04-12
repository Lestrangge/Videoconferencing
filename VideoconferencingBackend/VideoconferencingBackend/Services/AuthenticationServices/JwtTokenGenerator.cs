using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VideoconferencingBackend.Interfaces.Services.Authentication;
using VideoconferencingBackend.Models.DBModels;

namespace VideoconferencingBackend.Services.AuthenticationServices
{
    public class JwtTokenGenerator : ITokenGeneratorService
    {
        private readonly IConfiguration _config;
        public JwtTokenGenerator(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateToken(User credentials)
        {
            var identity = GetIdentity(credentials);
            if (identity == null)
                return null;
            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                issuer: _config["Issuer"],
                audience: _config["Audience"],
                notBefore: now,
                claims: identity.Claims,
                expires: now.Add(TimeSpan.FromSeconds(double.Parse(_config["Lifetime"]))),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config["TokenSalt"])),
                    SecurityAlgorithms.HmacSha256));
            return (new JwtSecurityTokenHandler()).WriteToken(jwt);
        }

        private ClaimsIdentity GetIdentity(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.UserGuid),
            };
            var claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
            return claimsIdentity;

        }
    }
}
