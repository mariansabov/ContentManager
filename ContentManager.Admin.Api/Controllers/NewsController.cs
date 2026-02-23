using ContentManager.Application.Features.Publications.News;
using ContentManager.Application.Features.Publications.News.Dto;
using ContentManager.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContentManager.Admin.Api.Controllers
{
    [Authorize(Roles = "Admin")]
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

        [HttpPatch("{id}")]
        public async Task<ActionResult> PublishNews(
            [FromRoute] Guid id
        )
        {
            await mediator.Send(new PublishNewsCommand(id));
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
