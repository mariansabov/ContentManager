using ContentManager.Application.Features.Auth;
using Microsoft.AspNetCore.Http;

namespace ContentManager.Infrastructure.Auth.Interfaces
{
    public interface IAuthTokenWriter
    {
        Task SetAsync(HttpContext httpContext,
            SignInResponse tokens);

        Task ClearAsync(HttpContext httpContext);
    }
}
