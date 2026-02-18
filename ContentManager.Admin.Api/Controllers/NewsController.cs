using ContentManager.Application.Features.Publications.News;
using ContentManager.Application.Features.Publications.News.Dto;
using MediatR;
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

        [HttpPatch]
        public async Task<ActionResult<Guid>> PublishNews(Guid id, [FromBody] PublishNewsCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }

            var newsId = await mediator.Send(command);
                return newsId;
        }

        [HttpDelete]
        public async Task<ActionResult<Guid>> Delete(Guid id)
        {
            var newsId = await mediator.Send(new DeleteNewsCommand(id));
            return newsId;
        }
    }
}
