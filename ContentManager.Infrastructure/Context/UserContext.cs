using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using ContentManager.Application.Common.Interfaces;
using ContentManager.Domain.Enums;

namespace ContentManager.Infrastructure.Context
{
    public class UserContext : IUserContext
    {
        public Guid Id { get; set; }

        public UserRole Role { get; set; }

        public UserContext(IHttpContextAccessor httpContextAccessor)
        {
            var httpContext = httpContextAccessor.HttpContext;

            if (httpContext is null)
            {
                return;
            }

            var userClaims = httpContext.User;

            var idClaim = userClaims.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!string.IsNullOrEmpty(idClaim) && Guid.TryParse(idClaim, out var parsedId))
            {
                Console.WriteLine($"Parsed user ID: {parsedId}");
                Id = parsedId;
            }
            else
            {
                Console.WriteLine($"No pars");
                Id = Guid.Empty;
            }

            var roleClaim = userClaims.FindFirstValue(ClaimTypes.Role);

            if (!string.IsNullOrEmpty(roleClaim) && Enum.TryParse<UserRole>(roleClaim, out var parsedRole))
            {
                Console.WriteLine($"Parsed user role: {parsedRole}");
                Role = parsedRole;
            }
        }
    }
}
