using ContentManager.Domain.Enums;

namespace ContentManager.Application.Features.Publications.News.Dto
{
    public record NewsPublicationDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = null!;

        public string Content { get; set; } = null!;

        public PublicationType Type { get; set; } = PublicationType.News;

        public PublicationStatus Status { get; set; } = PublicationStatus.Draft;

        public DateTime CreatedAt { get; set; }
        public DateTime? PublishedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public string AuthorUsername { get; set; } = string.Empty;
    }
}
