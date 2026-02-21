using ContentManager.Domain.Enums;

namespace ContentManager.Application.Common.Interfaces
{
    public interface IUserContext
    {
        public Guid Id { get; set; }

        public UserRole Role { get; set; }
    }
}
