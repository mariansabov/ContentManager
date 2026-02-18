using AutoMapper.QueryableExtensions;
using ContentManager.Application.Features.Publications.News.Dto;
using MediatR;
using AutoMapper;
using ContentManager.Application.Common.Interfaces;
using ContentManager.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace ContentManager.Application.Features.Publications.News
{
    public record GetPublishedNewsQuery : IRequest<List<NewsPublicationDto>>;

    public class GetPublishedNewsQueryHandler(IApplicationDatabaseContext context, IMapper mapper) : IRequestHandler<GetPublishedNewsQuery, List<NewsPublicationDto>>
    {
        public async Task<List<NewsPublicationDto>> Handle(GetPublishedNewsQuery request, CancellationToken cancellationToken)
        {
            var newsEntities = await context.Publications
                .Where(p => p.Type == PublicationType.News && p.Status == PublicationStatus.Published)
                .OrderByDescending(p => p.CreatedAt)
                .ProjectTo<NewsPublicationDto>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return newsEntities;
        }
    }
}
