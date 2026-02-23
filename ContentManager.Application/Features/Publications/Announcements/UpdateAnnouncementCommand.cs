using ContentManager.Application.Common.Interfaces;
using ContentManager.Domain.Enums;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ContentManager.Application.Features.Publications.Announcements
{
    public record UpdateAnnouncementCommand(
        Guid Id,
        string? Title,
        string? Content,
        int? HoursToLive
    ) : IRequest<Unit>;

    public class UpdateAnnouncementCommandValidator : AbstractValidator<UpdateAnnouncementCommand>
    {
        public UpdateAnnouncementCommandValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(200).When(x => x.Title is not null);

            RuleFor(x => x.Content).NotEmpty().When(x => x.Content is not null);

            RuleFor(x => x.HoursToLive)
                .GreaterThan(0)
                .LessThanOrEqualTo(168)
                .When(a => a.HoursToLive is not null);
        }
    }

    public class UpdateAnnouncementCommandHandler(IApplicationDatabaseContext context)
        : IRequestHandler<UpdateAnnouncementCommand, Unit>
    {
        public async Task<Unit> Handle(
            UpdateAnnouncementCommand request,
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

            if (request.Title is not null)
            {
                announcement.Title = request.Title;
            }

            if (request.Content is not null)
            {
                announcement.Content = request.Content;
            }

            if (request.HoursToLive is not null)
            {
                announcement.HoursToLive += request.HoursToLive.Value;
                var newExpirationTime = announcement.ExpiresAt?.AddHours(request.HoursToLive.Value);

                announcement.ExpiresAt = newExpirationTime;
            }

            announcement.UpdatedAt = DateTime.UtcNow;
            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
