using ContentManager.Application.Common.Interfaces;
using ContentManager.Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ContentManager.Application.Features.Auth
{
    public record SignInCommand(string Email, string Password) : IRequest<SignInResponse>;

    public class SignInCommandValidator : AbstractValidator<SignInCommand>
    {
        public SignInCommandValidator()
        {
            RuleFor(command => command.Email).NotEmpty().EmailAddress();
            RuleFor(command => command.Password).NotEmpty().MinimumLength(8).MaximumLength(100);
        }
    }

    public class SignInCommandHandler(
        IApplicationDatabaseContext context,
        IJwtTokenService jwt,
        IPasswordHasher<User> passwordHasher
    ) : IRequestHandler<SignInCommand, SignInResponse>
    {
        public async Task<SignInResponse> Handle(
            SignInCommand request,
            CancellationToken cancellationToken
        )
        {
            var user = await context.Users.FirstOrDefaultAsync(
                u => u.Email == request.Email,
                cancellationToken
            );

            if (user is null)
            {
                throw new Exception("Invalid credentials");
            }

            var verificationResult = passwordHasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                request.Password
            );

            if (verificationResult == PasswordVerificationResult.Failed)
            {
                throw new Exception("Invalid credentials");
            }

            var token = jwt.Generate(user);

            return new SignInResponse(token);
        }
    }
}
