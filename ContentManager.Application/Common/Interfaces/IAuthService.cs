using ContentManager.Application.Features.Auth;

namespace ContentManager.Application.Common.Interfaces
{
    public record LoginRequest(string Email, string Password);

    public interface IAuthService
    {
        Task<SignInResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken);
    }
}
