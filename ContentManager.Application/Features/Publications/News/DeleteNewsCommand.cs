using ContentManager.Application.Common.Interfaces;
using ContentManager.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ContentManager.Application.Features.Publications.News
{
    public record DeleteNewsCommand(Guid Id) : IRequest<Guid>;

    public class DeleteNewsCommandHandler(IApplicationDatabaseContext context, IUserContext userContext) : IRequestHandler<DeleteNewsCommand, Guid>
    {
        public async Task<Guid> Handle(DeleteNewsCommand request, CancellationToken cancellationToken)
        {

            var news = await context.Publications.SingleOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (news is null)
            {
                throw new KeyNotFoundException($"News with Id {request.Id} not found.");
            }

            if (news.AuthorId != userContext.Id && userContext.Role != UserRole.Admin)
            {
                throw new Exception("No permission.");
            }

            context.Publications.Remove(news);
            await context.SaveChangesAsync(cancellationToken);

            return request.Id;
        }
    }
}
