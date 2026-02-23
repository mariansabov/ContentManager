using ContentManager.Application.Common.Interfaces;
using ContentManager.Domain.Enums;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ContentManager.Application.Features.Users
{
    public record UpdateUserCommand(Guid Id, string? Username, string? Email, UserRole? Role)
        : IRequest<Unit>;

    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {
            RuleFor(c => c.Username).NotEmpty().MaximumLength(50).When(c => c.Username is not null);
            RuleFor(c => c.Email).NotEmpty().EmailAddress().When(c => c.Email is not null);
            RuleFor(x => x.Role)
                .IsInEnum()
                .WithMessage("Невалідна роль користувача")
                .When(c => c.Role is not null);
        }
    }

    public class UpdateUserCommandHandler(IApplicationDatabaseContext dbContext)
        : IRequestHandler<UpdateUserCommand, Unit>
    {
        public async Task<Unit> Handle(
            UpdateUserCommand request,
            CancellationToken cancellationToken
        )
        {
            var user =
                await dbContext.Users.FirstOrDefaultAsync(
                    u => u.Id == request.Id,
                    cancellationToken
                ) ?? throw new KeyNotFoundException($"Користувача з Id {request.Id} не знайдено.");

            if (request.Username is not null)
            {
                user.Username = request.Username;
            }

            if (request.Email is not null)
            {
                user.Email = request.Email;
            }

            if (request.Role is not null)
            {
                user.Role = request.Role.Value;
            }

            await dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
