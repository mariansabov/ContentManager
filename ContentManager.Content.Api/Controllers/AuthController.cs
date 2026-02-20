using ContentManager.Application.Features.Auth;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ContentManager.Content.Api.Controllers
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
            return Ok(userToken);
        }
    }
}
