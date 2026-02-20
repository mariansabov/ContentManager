using ContentManager.Application.Features.Publications.News;
using ContentManager.Application.Features.Publications.News.Dto;
using ContentManager.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContentManager.Admin.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NewsController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<NewsPublicationDto>>> GetAll()
        {
            var news = await mediator.Send(new GetAllNewsQuery());
            return Ok(news);
        }

        [HttpGet("published")]
        public async Task<ActionResult<List<NewsPublicationDto>>> GetPublished()
        {
            var news = await mediator.Send(new GetPublishedNewsQuery());
            return Ok(news);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateNewsCommand command)
        {
            var newsId = await mediator.Send(command);
            return Ok(newsId);
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch("{id}")]
        public async Task<ActionResult<Guid>> PublishNews(
            [FromRoute] Guid id,
            [FromBody] PublishNewsCommand command
        )
        {
            await mediator.Send(command with { Id = id });
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Guid>> Delete([FromRoute] Guid id)
        {
            var newsId = await mediator.Send(new DeleteNewsCommand(id));
            return newsId;
        }
    }
}
