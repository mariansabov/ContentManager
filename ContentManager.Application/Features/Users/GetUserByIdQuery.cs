using AutoMapper;
using AutoMapper.QueryableExtensions;
using ContentManager.Application.Common.Interfaces;
using ContentManager.Application.Features.Users.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ContentManager.Application.Features.Users
{
    public record GetUserByIdQuery(Guid Id) : IRequest<UserDto>;

    public class GetUserByIdQueryHandler(IApplicationDatabaseContext context, IMapper mapper)
        : IRequestHandler<GetUserByIdQuery, UserDto>
    {
        public async Task<UserDto> Handle(
            GetUserByIdQuery request,
            CancellationToken cancellationToken
        )
        {
            var userEntity = await context
                .Users.AsNoTracking()
                .Where(u => u.Id == request.Id)
                .ProjectTo<UserDto>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);

            return userEntity
                ?? throw new KeyNotFoundException($"User with ID {request.Id} not found.");
        }
    }
}
