using AutoMapper;
using AutoMapper.QueryableExtensions;
using ContentManager.Application.Common.Interfaces;
using ContentManager.Application.Features.Publications.News.Dto;
using ContentManager.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ContentManager.Application.Features.Publications.News
{
    public record GetPublishedNewsByIdQuery(Guid Id) : IRequest<NewsPublicationDto>;

    public class GetPublishedNewsByIdQueryHandler(
        IApplicationDatabaseContext context,
        IMapper mapper
    ) : IRequestHandler<GetPublishedNewsByIdQuery, NewsPublicationDto>
    {
        public async Task<NewsPublicationDto> Handle(
            GetPublishedNewsByIdQuery request,
            CancellationToken cancellationToken
        )
        {
            var newsEntity = await context
                .Publications.AsNoTracking()
                .Where(p =>
                    p.Type == PublicationType.News
                    && p.Status == PublicationStatus.Published
                    && p.Id == request.Id
                )
                .ProjectTo<NewsPublicationDto>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);

            return newsEntity
                ?? throw new KeyNotFoundException($"News with ID {request.Id} not found.");
        }
    }
}
