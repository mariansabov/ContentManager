using AutoMapper;
using AutoMapper.QueryableExtensions;
using ContentManager.Application.Common.Interfaces;
using ContentManager.Application.Features.Publications.News.Dto;
using ContentManager.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ContentManager.Application.Features.Publications.News
{
    public record GetPublishedNewsQuery : IRequest<List<NewsPublicationDto>>;

    public class GetPublishedNewsQueryHandler(IApplicationDatabaseContext context, IMapper mapper)
        : IRequestHandler<GetPublishedNewsQuery, List<NewsPublicationDto>>
    {
        public async Task<List<NewsPublicationDto>> Handle(
            GetPublishedNewsQuery request,
            CancellationToken cancellationToken
        )
        {
            var newsEntities = await context
                .Publications.AsNoTracking()
                .Where(publication => publication.Status == PublicationStatus.Published
                )
                .OrderByDescending(p => p.CreatedAt)
                .ProjectTo<NewsPublicationDto>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return newsEntities;
        }
    }
}
