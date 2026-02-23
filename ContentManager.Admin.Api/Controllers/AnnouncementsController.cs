using ContentManager.Application.Features.Publications.Announcements;
using ContentManager.Application.Features.Publications.Announcements.Dto;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContentManager.Admin.Api.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class AnnouncementsController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<AnnouncementPublicationDto>>> GetAll()
        {
            var announcements = await mediator.Send(new GetAnnouncementsQuery());
            return Ok(announcements);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<AnnouncementPublicationDto>> GetById([FromRoute] Guid id)
        {
            var announcement = await mediator.Send(new GetAnnouncementByIdQuery(id));
            return Ok(announcement);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateAnnouncementCommand command)
        {
            var announcementId = await mediator.Send(command);
            return Ok(announcementId);
        }

        [HttpPatch]
        [Route("{id}")]
        public async Task<ActionResult> Update(
            [FromRoute] Guid id,
            [FromBody] UpdateAnnouncementCommand command
        )
        {
            await mediator.Send(command with { Id = id });
            return NoContent();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> Delete([FromRoute] Guid id)
        {
            await mediator.Send(new DeleteAnnouncementCommand(id));
            return NoContent();
        }
    }
}
