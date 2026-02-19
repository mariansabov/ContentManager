using System.IdentityModel.Tokens.Jwt;
using ContentManager.Application.Common.Interfaces;
using ContentManager.Domain.Entities;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace ContentManager.Infrastructure.Services
{
    public class JwtTokenService(IConfiguration configuration) : IJwtTokenService
    {
        public string Generate(User user)
        {
            var jwt = configuration.GetSection("Jwt");

            var claims = new[]
            {
                new Claim("userId", user.Id.ToString()),
                new Claim("username", user.Username),
                new Claim("email", user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwt["Key"]!)
            );

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwt["Issuer"],
                audience: jwt["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    int.Parse(jwt["AccessTtlMinutes"]!)
                ),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
