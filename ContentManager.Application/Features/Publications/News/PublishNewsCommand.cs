using ContentManager.Application.Common.Interfaces;
using ContentManager.Domain.Enums;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ContentManager.Application.Features.Publications.News
{
    public record PublishNewsCommand(Guid Id) : IRequest<Guid>;

    public class PublishNewsCommandValidator : AbstractValidator<PublishNewsCommand>
    {
        public PublishNewsCommandValidator()
        {
            RuleFor(c => c.Id).NotEmpty().WithMessage("News id is required.");
        }
    }

    public class PublishNewsCommandHandler(IApplicationDatabaseContext context)
        : IRequestHandler<PublishNewsCommand, Guid>
    {
        public async Task<Guid> Handle(
            PublishNewsCommand request,
            CancellationToken cancellationToken
        )
        {
            var now = DateTime.UtcNow;

            var news =
                await context
                    .Publications.Where(n => n.Id == request.Id)
                    .SingleOrDefaultAsync(cancellationToken)
                ?? throw new KeyNotFoundException($"News with id {request.Id} not found.");

            news.Status = PublicationStatus.Published;
            news.PublishedAt = now;
            news.UpdatedAt = now;

            await context.SaveChangesAsync(cancellationToken);

            return request.Id;
        }
    }
}
