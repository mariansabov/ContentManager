using ContentManager.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentManager.Application.Features.Publications.News.Dto
{
    public record NewsPublicationDto
    {
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
