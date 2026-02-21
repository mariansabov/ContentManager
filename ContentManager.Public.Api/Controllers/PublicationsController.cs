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
            var news = await mediator.Send(new GetPublishedNewsQuery());
            return Ok(news);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<NewsPublicationDto>> GetPublishedNewsById([FromRoute] Guid id)
        {
            var news = await mediator.Send(new GetPublishedNewsByIdQuery(id));
            return Ok(news);
        }
    }
}
