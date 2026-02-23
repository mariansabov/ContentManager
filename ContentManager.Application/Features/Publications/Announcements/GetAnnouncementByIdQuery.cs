using AutoMapper;
using AutoMapper.QueryableExtensions;
using ContentManager.Application.Common.Interfaces;
using ContentManager.Application.Features.Publications.Announcements.Dto;
using ContentManager.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ContentManager.Application.Features.Publications.Announcements
{
    public record GetAnnouncementByIdQuery(Guid Id) : IRequest<AnnouncementPublicationDto>;

    public class GetAnnouncementByIdQueryHandler(
        IApplicationDatabaseContext context,
        IMapper mapper
    ) : IRequestHandler<GetAnnouncementByIdQuery, AnnouncementPublicationDto>
    {
        public async Task<AnnouncementPublicationDto> Handle(
            GetAnnouncementByIdQuery request,
            CancellationToken cancellationToken
        )
        {
            var announcementEntity = await context
                .Publications.AsNoTracking()
                .Where(p => p.Type == PublicationType.Announcement && p.Id == request.Id)
                .ProjectTo<AnnouncementPublicationDto>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);

            return announcementEntity
                ?? throw new KeyNotFoundException($"Announcement with ID {request.Id} not found.");
        }
    }
}
