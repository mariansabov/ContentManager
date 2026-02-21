using System.Security.Claims;
using ContentManager.Application.Common.Interfaces;
using ContentManager.Application.Features.Auth;
using ContentManager.Domain.Entities;
using ContentManager.Infrastructure.Auth.Interfaces;
using ContentManager.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;

namespace ContentManager.Infrastructure.Services
{
    public sealed class AuthService(
        DatabaseContext dbContext,
        IJwtTokenService jwt,
        IPasswordHasher<User> passwordHasher,
        IHttpContextAccessor httpContextAccessor,
        IAuthTokenWriter writer
    ) : IAuthService
    {
        public async Task<SignInResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken)
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(
                u => u.Email == request.Email,
                cancellationToken
            );

            if (user is null)
            {
                throw new Exception("Invalid credentials");
            }

            var verificationResult = passwordHasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                request.Password
            );

            if (verificationResult == PasswordVerificationResult.Failed)
            {
                throw new Exception("Invalid credentials");
            }

            if (verificationResult == PasswordVerificationResult.SuccessRehashNeeded)
            {
                user.PasswordHash = passwordHasher.HashPassword(user, request.Password);
            }

            var token = jwt.Generate(user);

            var accessToken = new SignInResponse(token);

            await dbContext.SaveChangesAsync(cancellationToken);
            await writer.SetAsync(httpContextAccessor.HttpContext!, accessToken);

            return accessToken;
        }
    }
}
