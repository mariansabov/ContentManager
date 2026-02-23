using AutoMapper;
using AutoMapper.QueryableExtensions;
using ContentManager.Application.Common.Interfaces;
using ContentManager.Application.Features.Publications.News.Dto;
using ContentManager.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ContentManager.Application.Features.Publications.News
{
    public record GetAllNewsQuery : IRequest<List<NewsPublicationDto>>;

    public class GetAllNewsQueryHandler(IApplicationDatabaseContext context, IMapper mapper)
        : IRequestHandler<GetAllNewsQuery, List<NewsPublicationDto>>
    {
        public async Task<List<NewsPublicationDto>> Handle(
            GetAllNewsQuery request,
            CancellationToken cancellationToken
        )
        {
            var newsEntities = await context
                .Publications.AsNoTracking()
                .Where(p =>
                    p.Type == PublicationType.News
                    && (p.ExpiresAt == null || p.ExpiresAt > DateTime.UtcNow)
                )
                .OrderByDescending(p => p.CreatedAt)
                .ProjectTo<NewsPublicationDto>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return newsEntities;
        }
    }
}
