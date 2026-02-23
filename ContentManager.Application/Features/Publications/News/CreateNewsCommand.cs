using ContentManager.Application.Common.Interfaces;
using ContentManager.Domain.Enums;
using FluentValidation;
using MediatR;

namespace ContentManager.Application.Features.Publications.News
{
    public record CreateNewsCommand(string Title, string Content) : IRequest<Unit>;

    public class CreateNewsCommandValidator : AbstractValidator<CreateNewsCommand>
    {
        public CreateNewsCommandValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Content).NotEmpty();
        }
    }

    public class CreateNewsCommandHandler(IApplicationDatabaseContext dbContext, IUserContext userContext)
        : IRequestHandler<CreateNewsCommand, Unit>
    {
        public async Task<Unit> Handle(
            CreateNewsCommand request,
            CancellationToken cancellationToken
        )
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
                AuthorId = userContext.Id,
            };

            await dbContext.Publications.AddAsync(newsEntity, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
