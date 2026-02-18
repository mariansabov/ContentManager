using ContentManager.Application.Common.Interfaces;
using ContentManager.Domain.Enums;
using FluentValidation;
using MediatR;

namespace ContentManager.Application.Features.Publications.News
{
    public record CreateNewsCommand(
        string Title,
        string Content,
        Guid AuthorId) : IRequest<Guid>;

    public class CreateNewsCommandValidator : AbstractValidator<CreateNewsCommand>
    {
        public CreateNewsCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(200);
            RuleFor(x => x.Content)
                .NotEmpty();
            RuleFor(x => x.AuthorId)
                .NotEmpty();
        }
    }

    public class CreateNewsCommandHandler(IApplicationDatabaseContext context)
        : IRequestHandler<CreateNewsCommand, Guid>
    {
        public async Task<Guid> Handle(CreateNewsCommand request, CancellationToken cancellationToken)
        {
            var now = DateTime.UtcNow;

            var newsEntity = new Domain.Entities.Publication
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Content = request.Content,
                Status = PublicationStatus.Draft,
                Type = PublicationType.News,
                CreatedAt = now,
                UpdatedAt = now,
                AuthorId = request.AuthorId
            };

            await context.Publications.AddAsync(newsEntity, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            return newsEntity.Id;
        }
    }
}