using ContentManager.Application.Common.Interfaces;
using ContentManager.Domain.Enums;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ContentManager.Application.Features.Publications.News
{
    public record UpdateNewsCommand(Guid Id, string? Title, string? Content) : IRequest<Unit>;

    public class UpdateNewsCommandValidator : AbstractValidator<UpdateNewsCommand>
    {
        public UpdateNewsCommandValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(200).When(x => x.Title is not null);
            RuleFor(x => x.Content).NotEmpty().When(x => x.Content is not null);
        }
    }

    public class UpdateNewsCommandHandler(
        IApplicationDatabaseContext dbContext,
        IUserContext userContext
    ) : IRequestHandler<UpdateNewsCommand, Unit>
    {
        public async Task<Unit> Handle(
            UpdateNewsCommand request,
            CancellationToken cancellationToken
        )
        {
            var news = await dbContext.Publications.SingleOrDefaultAsync(
                p => p.Id == request.Id,
                cancellationToken
            );

            if (news is null)
            {
                throw new KeyNotFoundException($"News with Id {request.Id} not found.");
            }

            if (news.AuthorId != userContext.Id && userContext.Role != UserRole.Admin)
            {
                throw new Exception("No permission.");
            }

            if (news.Status == PublicationStatus.Published && userContext.Role != UserRole.Admin)
            {
                throw new InvalidOperationException("Cannot edit published news.");
            }

            if (request.Title is not null)
            {
                news.Title = request.Title;
            }

            if (request.Content is not null)
            {
                news.Content = request.Content;
            }

            news.UpdatedAt = DateTime.UtcNow;
            await dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
