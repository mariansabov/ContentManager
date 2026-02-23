using ContentManager.Application.Features.Publications.News;
using ContentManager.Application.Features.Publications.News.Dto;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ContentManager.Public.Api.Controllers
{
    [ApiController]
    [Route("api/publications")]
    public class PublicationsController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<NewsPublicationDto>>> GetPublishedNews()
        {
            return await mediator.Send(new GetPublishedNewsQuery());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<NewsPublicationDto>> GetPublishedNewsById([FromRoute] Guid id)
        {
            return await mediator.Send(new GetPublishedNewsByIdQuery(id));
        }
    }
}
