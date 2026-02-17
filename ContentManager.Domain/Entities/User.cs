using ContentManager.Domain.Common;
using ContentManager.Domain.Enums;

namespace ContentManager.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Username { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public UserRole Role { get; set; } = UserRole.Author;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Publication> Publications { get; set; } = new List<Publication>();
    }
}
