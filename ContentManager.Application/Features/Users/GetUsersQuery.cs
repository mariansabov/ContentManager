using AutoMapper;
using AutoMapper.QueryableExtensions;
using ContentManager.Application.Common.Interfaces;
using ContentManager.Application.Features.Users.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ContentManager.Application.Features.Users
{
    public record GetUsersQuery : IRequest<List<UserDto>>;

    public class GetUsersQueryHandler(
        IApplicationDatabaseContext dbContext,
        IUserContext userContext,
        IMapper mapper
    ) : IRequestHandler<GetUsersQuery, List<UserDto>>
    {
        public async Task<List<UserDto>> Handle(
            GetUsersQuery request,
            CancellationToken cancellationToken
        )
        {
            var users = await dbContext
                .Users.AsNoTracking()
                .ProjectTo<UserDto>(mapper.ConfigurationProvider)
                .Where(u => u.Id != userContext.Id)
                .ToListAsync(cancellationToken);

            return users;
        }
    }
}
