using ContentManager.Application.Common.Interfaces;
using FluentValidation;
using MediatR;

namespace ContentManager.Application.Features.Auth
{
    public record SignInCommand(string Email, string Password) : IRequest<SignInResponse>;

    public record SignInResponse(string AccessToken);

    public class SignInCommandValidator : AbstractValidator<SignInCommand>
    {
        public SignInCommandValidator()
        {
            RuleFor(command => command.Email).NotEmpty().EmailAddress();
            RuleFor(command => command.Password).NotEmpty().MinimumLength(8).MaximumLength(100);
        }
    }

    public class SignInCommandHandler(IAuthService authService)
        : IRequestHandler<SignInCommand, SignInResponse>
    {
        public async Task<SignInResponse> Handle(
            SignInCommand request,
            CancellationToken cancellationToken
        )
        {
            var loginRequest = new LoginRequest(request.Email, request.Password);
            var result = await authService.LoginAsync(loginRequest, cancellationToken);

            return new SignInResponse(result.AccessToken);
        }
    }
}
