using ContentManager.Application.Common.Interfaces;
using ContentManager.Domain.Entities;
using ContentManager.Domain.Enums;
using FluentValidation;
using MediatR;

namespace ContentManager.Application.Features.Publications.Announcements
{
    public record CreateAnnouncementCommand(string Title, string Content, int HoursToLive)
        : IRequest<Guid>;

    public class CreateAnnouncementCommandValidator : AbstractValidator<CreateAnnouncementCommand>
    {
        public CreateAnnouncementCommandValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(200);

            RuleFor(x => x.Content).NotEmpty();

            RuleFor(x => x.HoursToLive).GreaterThan(0).LessThanOrEqualTo(168); // Max 7 days
        }
    }

    public class CreateAnnouncementCommandHandler(
        IApplicationDatabaseContext dbContext,
        IUserContext userContext
    ) : IRequestHandler<CreateAnnouncementCommand, Guid>
    {
        public async Task<Guid> Handle(
            CreateAnnouncementCommand request,
            CancellationToken cancellationToken
        )
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
                AuthorId = userContext.Id,
            };

            await dbContext.Publications.AddAsync(announcementEntity, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            return announcementEntity.Id;
        }
    }
}
