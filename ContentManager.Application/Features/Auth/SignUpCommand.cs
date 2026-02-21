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
        string PasswordConfirmation,
        UserRole Role
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
            var isEmailExists = await context.Users
                .AnyAsync(u => u.Email == request.Email, cancellationToken);
            if (isEmailExists)
            {
                throw new Exception("Email already registered");
            }

            var isUsernameExists = await context.Users
                .AnyAsync(u => u.Username == request.Username, cancellationToken);
            if (isUsernameExists)
            {
                throw new Exception("Username already registered");
            } 

            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = request.Username,
                Email = request.Email,
                CreatedAt = DateTime.UtcNow,
                Role = request.Role
            };

            user.PasswordHash = new PasswordHasher<User>().HashPassword(user, request.Password);

            context.Users.Add(user);
            await context.SaveChangesAsync(cancellationToken);

            var token = jwt.Generate(user);

            return token;
        }
    }
}
