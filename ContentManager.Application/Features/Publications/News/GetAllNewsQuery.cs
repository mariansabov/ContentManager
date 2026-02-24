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

    public class GetAllNewsQueryHandler(
        IApplicationDatabaseContext dbContext,
        IUserContext? userContext,
        IMapper mapper
    ) : IRequestHandler<GetAllNewsQuery, List<NewsPublicationDto>>
    {
        public async Task<List<NewsPublicationDto>> Handle(
            GetAllNewsQuery request,
            CancellationToken cancellationToken
        )
        {
            var currentUserId = userContext?.Id;
            var currentUserRole = userContext?.Role;

            var getNewsQuery = dbContext.Publications.AsNoTracking().AsQueryable();

            if (currentUserId != null && currentUserRole != UserRole.Admin)
            {
                getNewsQuery = getNewsQuery.Where(p =>
                    p.Type == PublicationType.News && p.AuthorId == currentUserId
                );
            }
            else
            {
                getNewsQuery = getNewsQuery.Where(p => p.Type == PublicationType.News);
            }

            var newsEntities = await getNewsQuery
                .OrderByDescending(p => p.CreatedAt)
                .ProjectTo<NewsPublicationDto>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return newsEntities;
        }
    }
}
