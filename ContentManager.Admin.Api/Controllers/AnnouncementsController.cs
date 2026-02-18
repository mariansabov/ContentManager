using ContentManager.Application.Features.Publications.Announcements;
using ContentManager.Application.Features.Publications.Announcements.Dto;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ContentManager.Admin.Api.Controllers
{
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

        [HttpPost]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateAnnouncementCommand command)
        {
            var announcementId = await mediator.Send(command);
            return Ok(announcementId);
        }
    }
}
