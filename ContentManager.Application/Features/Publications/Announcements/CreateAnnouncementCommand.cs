using ContentManager.Application.Common.Interfaces;
using ContentManager.Domain.Entities;
using ContentManager.Domain.Enums;
using MediatR;

namespace ContentManager.Application.Features.Publications.Announcements
{
    public record CreateAnnouncementCommand(
        string Title,
        string Content,
        int HoursToLive,
        Guid AuthorId) : IRequest<Guid>;

    public class CreateAnnouncementCommandHandler(IApplicationDatabaseContext context) : IRequestHandler<CreateAnnouncementCommand, Guid>
    {
        public async Task<Guid> Handle(CreateAnnouncementCommand request, CancellationToken cancellationToken)
        {
            var now = DateTime.UtcNow;

            var announcementEntity = new Publication
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Content = request.Content,
                Type = PublicationType.Announcement,
                HoursToLive = request.HoursToLive,
                CreatedAt = now,
                UpdatedAt = now,
                ExpiresAt = now.AddHours(request.HoursToLive),
                AuthorId = request.AuthorId
            };

            await context.Publications.AddAsync(announcementEntity, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            return announcementEntity.Id;
        }
    }
}