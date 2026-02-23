using AutoMapper;
using AutoMapper.QueryableExtensions;
using ContentManager.Application.Common.Interfaces;
using ContentManager.Application.Features.Publications.News.Dto;
using ContentManager.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ContentManager.Application.Features.Publications.News
{
    public record GetNewsByIdQuery(Guid Id) : IRequest<NewsPublicationDto>;

    public class GetNewsByIdQueryHandler(IApplicationDatabaseContext dbContext, IMapper mapper)
        : IRequestHandler<GetNewsByIdQuery, NewsPublicationDto>
    {
        public async Task<NewsPublicationDto> Handle(
            GetNewsByIdQuery request,
            CancellationToken cancellationToken
        )
        {
            var news = await dbContext
                .Publications.AsNoTracking()
                .Where(p => p.Id == request.Id && p.Type == PublicationType.News)
                .ProjectTo<NewsPublicationDto>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);

            return news ?? throw new KeyNotFoundException($"News with ID {request.Id} not found.");
        }
    }
}
