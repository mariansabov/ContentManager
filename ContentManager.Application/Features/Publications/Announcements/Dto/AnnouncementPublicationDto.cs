using ContentManager.Domain.Enums;

namespace ContentManager.Application.Features.Publications.Announcements.Dto
{
    public record AnnouncementPublicationDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;

        public string Content { get; set; } = null!;

        public PublicationType Type { get; set; } = PublicationType.Announcement;

        public int? HoursToLive { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? ExpiresAt { get; set; }

        public string AuthorUsername { get; set; } = string.Empty;
    }
}
