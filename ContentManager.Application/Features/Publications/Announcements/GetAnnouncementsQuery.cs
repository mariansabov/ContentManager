using AutoMapper;
using AutoMapper.QueryableExtensions;
using ContentManager.Application.Common.Interfaces;
using ContentManager.Application.Features.Publications.Announcements.Dto;
using ContentManager.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ContentManager.Application.Features.Publications.Announcements
{
    public record GetAnnouncementsQuery : IRequest<List<AnnouncementPublicationDto>>;

    public class GetAnnouncementsQueryHandler(IApplicationDatabaseContext context, IMapper mapper)
        : IRequestHandler<GetAnnouncementsQuery, List<AnnouncementPublicationDto>>
    {
        public async Task<List<AnnouncementPublicationDto>> Handle(
            GetAnnouncementsQuery request,
            CancellationToken cancellationToken
        )
        {
            var now = DateTime.UtcNow;

            var announcements = await context
                .Publications.Where(p =>
                    p.ExpiresAt > now && p.Type == PublicationType.Announcement
                )
                .ProjectTo<AnnouncementPublicationDto>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return announcements;
        }
    }
}
