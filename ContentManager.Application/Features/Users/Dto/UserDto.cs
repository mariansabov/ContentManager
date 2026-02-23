using ContentManager.Domain.Entities;
using ContentManager.Domain.Enums;

namespace ContentManager.Application.Features.Users.Dto
{
    public record UserDto
    {
        public Guid Id { get; set; }

        public string Username { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public UserRole Role { get; set; } = UserRole.Author;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public List<UserPublicationsDto> Publications { get; set; } = new();
    }

    public record UserPublicationsDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = string.Empty;
    }
}
