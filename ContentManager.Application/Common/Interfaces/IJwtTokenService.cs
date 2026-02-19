using ContentManager.Domain.Entities;

namespace ContentManager.Application.Common.Interfaces
{
    public interface IJwtTokenService
    {
        string Generate(User user);
    }
}
