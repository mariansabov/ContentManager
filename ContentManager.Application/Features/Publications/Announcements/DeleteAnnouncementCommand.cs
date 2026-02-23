using ContentManager.Application.Common.Interfaces;
using ContentManager.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ContentManager.Application.Features.Publications.Announcements
{
    public record DeleteAnnouncementCommand(Guid Id) : IRequest<Unit>;

    public class DeleteAnnouncementCommandHandler(IApplicationDatabaseContext context)
        : IRequestHandler<DeleteAnnouncementCommand, Unit>
    {
        public async Task<Unit> Handle(
            DeleteAnnouncementCommand request,
            CancellationToken cancellationToken
        )
        {
            var announcement =
                await context.Publications.FirstOrDefaultAsync(
                    a => a.Id == request.Id,
                    cancellationToken
                )
                ?? throw new KeyNotFoundException($"Announcement with id {request.Id} not found.");

            if (announcement.Type != PublicationType.Announcement)
            {
                throw new InvalidOperationException(
                    $"Publication with id {request.Id} is not an announcement."
                );
            }

            context.Publications.Remove(announcement);
            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
