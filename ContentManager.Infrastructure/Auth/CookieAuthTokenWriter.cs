using ContentManager.Application.Features.Auth;
using ContentManager.Infrastructure.Auth.Interfaces;
using ContentManager.Infrastructure.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace ContentManager.Infrastructure.Auth
{
    public class CookieAuthTokenWriter(
        IOptionsMonitor<JwtOptions> options) : IAuthTokenWriter
    {
        private readonly JwtOptions _jwt = options.CurrentValue;
        public Task SetAsync(HttpContext httpContext, SignInResponse userToken)
        {
            httpContext.Response.Cookies.Append(
                "access_token",
                userToken.AccessToken,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true, 
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.UtcNow.AddMinutes(_jwt.AccessTtlMinutes),
                }
            );

            return Task.CompletedTask;
        }

        public Task ClearAsync(HttpContext httpContext)
        {
            throw new NotImplementedException();
        }
    }
}
