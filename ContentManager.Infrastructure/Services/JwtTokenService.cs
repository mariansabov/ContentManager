using System.IdentityModel.Tokens.Jwt;
using ContentManager.Application.Common.Interfaces;
using ContentManager.Domain.Entities;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.Text;
using ContentManager.Infrastructure.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ContentManager.Infrastructure.Services
{
    public class JwtTokenService(IOptions<JwtOptions> jwt) : IJwtTokenService
    {
        private readonly JwtOptions _jwt = jwt.Value;

        public string Generate(User user)
        {
            var now = DateTime.UtcNow;

            var claims = new[]
            {
                new Claim("userId", user.Id.ToString()),
                new Claim("email", user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_jwt.Key)
            );

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                notBefore: now,
                expires: now.AddMinutes(_jwt.AccessTtlMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
