using ContentManager.Application.Features.Auth;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ContentManager.Admin.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController(IMediator mediator) : ControllerBase
    {
        [HttpPost("registration")]
        public async Task<IActionResult> Registration([FromBody] SignUpCommand command)
        {
            var userToken = await mediator.Send(command);
            return Ok(userToken);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] SignInCommand command)
        {
            var userToken = await mediator.Send(command);

            Response.Cookies.Append(
                "access_token",
                userToken.AccessToken,
                new CookieOptions
                {
                    HttpOnly = true, // ❗ JS не бачить cookie
                    Secure = true, // тільки HTTPS
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.UtcNow.AddMinutes(60),
                }
            );

            return Ok(userToken);
        }
    }
}
