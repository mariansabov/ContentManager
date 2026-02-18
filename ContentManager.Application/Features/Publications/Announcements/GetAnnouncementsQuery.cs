
using ContentManager.Application.Common.Interfaces;
using ContentManager.Application.Features.Publications.Announcements.Dto;
using ContentManager.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ContentManager.Application.Features.Publications.Announcements
{
    public record GetAnnouncementsQuery : IRequest<List<AnnouncementPublicationDto>>;

    public class GetAnnouncementsQueryHandler(IApplicationDatabaseContext context)
        : IRequestHandler<GetAnnouncementsQuery, List<AnnouncementPublicationDto>>
    {
        public async Task<List<AnnouncementPublicationDto>> Handle(GetAnnouncementsQuery request,
            CancellationToken cancellationToken)
        {
            var now = DateTime.UtcNow;

            var announcements = await context.Publications
                .Where(p => p.ExpiresAt > now
                    && p.Type == PublicationType.Announcement)
                .Select(p => new AnnouncementPublicationDto
                {
                    Title = p.Title,
                    Content = p.Content,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt,
                    ExpiresAt = p.ExpiresAt,
                    HoursToLive = p.HoursToLive,
                    AuthorUsername = p.Author.Username
                })
                .ToListAsync(cancellationToken);

            return announcements;
        }
    }
}