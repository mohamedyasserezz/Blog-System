using AutoMapper;
using BlogSystem.Core.Models;
using BlogSystem.Domain.Enities;

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
