using ContentManager.Domain.Common;
using ContentManager.Domain.Enums;

namespace ContentManager.Domain.Entities
{
    public class Publication : BaseEntity
    {
        public string Title { get; set; } = null!;

        public string Content { get; set; } = null!;

        public PublicationType Type { get; set; } = PublicationType.News;

        public PublicationStatus Status { get; set; } = PublicationStatus.Draft;

        public int? HoursToLive { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? PublishedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? ExpiresAt { get; set; }

        public Guid AuthorId { get; set; }
        public User Author { get; set; } = null!;
    }
}
