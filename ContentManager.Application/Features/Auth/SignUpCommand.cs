using ContentManager.Application.Common.Interfaces;
using ContentManager.Domain.Entities;
using ContentManager.Domain.Enums;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ContentManager.Application.Features.Auth
{
    public record SignUpCommand(
        string Username,
        string Email,
        string Password,
        string PasswordConfirmation
    ) : IRequest<string>;

    public class SignUpCommandValidator : AbstractValidator<SignUpCommand>
    {
        public SignUpCommandValidator()
        {
            RuleFor(command => command.Username).NotEmpty().MinimumLength(5).MaximumLength(100);
            RuleFor(command => command.Email).NotEmpty().EmailAddress();
            RuleFor(command => command.Password).NotEmpty().MinimumLength(8).MaximumLength(100);

            RuleFor(command => command.PasswordConfirmation)
                .NotEmpty()
                .Equal(command => command.Password)
                .WithMessage("Password confirmation does not match the password.");
        }
    }

    public class SignUpCommandHandler(IApplicationDatabaseContext context, IJwtTokenService jwt)
        : IRequestHandler<SignUpCommand, string>
    {
        public async Task<string> Handle(
            SignUpCommand request,
            CancellationToken cancellationToken
        )
        {
            var isExists = await context.Users
                .AnyAsync(x => x.Email == request.Email, cancellationToken);

            if (isExists)
            {
                throw new Exception("Email already registered");
            }

            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = request.Username,
                Email = request.Email,
                Role = UserRole.Author
            };

            user.PasswordHash = new PasswordHasher<User>().HashPassword(user, request.Password);

            context.Users.Add(user);
            await context.SaveChangesAsync(cancellationToken);

            var token = jwt.Generate(user);

            return token;
        }
    }
}
