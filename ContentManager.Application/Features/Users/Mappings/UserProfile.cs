using AutoMapper;
using ContentManager.Application.Features.Users.Dto;
using ContentManager.Domain.Entities;

namespace ContentManager.Application.Features.Users.Mappings
{
    public class UserProfile : Profile
    {
        public  UserProfile()
        {
            CreateMap<User, UserDto>();

            CreateMap<Publication, UserPublicationsDto>();
        }
    }
}
