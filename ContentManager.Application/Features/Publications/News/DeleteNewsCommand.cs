using ContentManager.Application.Common.Interfaces;
using MediatR;

namespace ContentManager.Application.Features.Publications.News
{
    public record DeleteNewsCommand(Guid Id) : IRequest<Guid>;

    public class DeleteNewsCommandHandler(IApplicationDatabaseContext context) : IRequestHandler<DeleteNewsCommand, Guid>
    {
        public async Task<Guid> Handle(DeleteNewsCommand request, CancellationToken cancellationToken)
        {
            var news = await context.Publications.FindAsync([request.Id], cancellationToken);

            if (news is null)
            {
                throw new KeyNotFoundException($"News with Id {request.Id} not found.");
            }

            context.Publications.Remove(news);
            await context.SaveChangesAsync(cancellationToken);

            return request.Id;
        }
    }
}
