using ContentManager.Application.Features.Publications.News;
using ContentManager.Application.Features.Publications.News.Dto;
using ContentManager.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContentManager.Content.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PublicationsController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<NewsPublicationDto>>> GetAll()
        {
            var news = await mediator.Send(new GetAllNewsQuery());
            return Ok(news);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<NewsPublicationDto>> GetById([FromRoute] Guid id)
        {
            var news = await mediator.Send(new GetNewsByIdQuery(id));
            return Ok(news);
        }

        [HttpGet("published")]
        public async Task<ActionResult<List<NewsPublicationDto>>> GetPublished()
        {
            var news = await mediator.Send(new GetPublishedNewsQuery());
            return Ok(news);
        }

        [HttpGet("published/{id}")]
        public async Task<ActionResult<NewsPublicationDto>> GetPublishedById([FromRoute] Guid id)
        {
            var news = await mediator.Send(new GetPublishedNewsByIdQuery(id));
            return Ok(news);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateNewsCommand command)
        {
            var newsId = await mediator.Send(command);
            return Ok(newsId);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<Guid>> Update([FromRoute] Guid id, [FromBody] UpdateNewsCommand command)
        {
            await mediator.Send(command with { Id = id });
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Guid>> Delete([FromRoute] Guid id)
        {
            var newsId = await mediator.Send(new DeleteNewsCommand(id));
            return newsId;
        }
    }
}
