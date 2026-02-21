using ContentManager.Application.Features.Auth;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContentManager.Admin.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController(IMediator mediator) : ControllerBase
    {
        [Authorize(Roles = "Admin")]
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
