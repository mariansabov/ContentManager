using ContentManager.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ContentManager.Application.Features.Users
{
    public record DeleteUserCommand(Guid Id) : IRequest<Unit>;

    public class DeleteUserCommandHandler(IApplicationDatabaseContext dbContext)
        : IRequestHandler<DeleteUserCommand, Unit>
    {
        public async Task<Unit> Handle(
            DeleteUserCommand request,
            CancellationToken cancellationToken
        )
        {
            var user =
                await dbContext.Users.FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken)
                ?? throw new KeyNotFoundException($"User with id {request.Id} not found.");

            dbContext.Users.Remove(user);
            await dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}