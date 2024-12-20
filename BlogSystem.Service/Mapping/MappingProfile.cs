using AutoMapper;
using BlogSystem.Domain.Enities;
using BlogSystem.Shared.Models.Posts;

namespace BlogSystem.Service.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Post, PostToCreateDto>()
                .ReverseMap();
        }
    }
}
