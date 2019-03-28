using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using VideoconferencingBackend.Interfaces;
using VideoconferencingBackend.Models.DBModels;

namespace VideoconferencingBackend.Services
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
            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.Name)
                };
                ClaimsIdentity claimsIdentity =
                    new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                        ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }

            return null;
        }
    }
}
