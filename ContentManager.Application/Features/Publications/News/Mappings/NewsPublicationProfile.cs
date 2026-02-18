using AutoMapper;
using ContentManager.Application.Features.Publications.News.Dto;
using ContentManager.Domain.Entities;

namespace ContentManager.Application.Features.Publications.News.Mappings
{
    public class NewsPublicationProfile : Profile
    {
        public NewsPublicationProfile()
        {
            CreateMap<Publication, NewsPublicationDto>()
                .ForMember(dest => dest.AuthorUsername, 
                    opt => opt.MapFrom(src => src.Author.Username));
        }
    }
}
