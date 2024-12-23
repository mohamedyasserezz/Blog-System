using AutoMapper;
using BlogSystem.Domain.Enities;
using BlogSystem.Shared.Models.Authentication;
using BlogSystem.Shared.Models.Posts;

namespace BlogSystem.Service.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<PostRequest, Post>()
                .ForMember(src =>src.Tags , config => config.MapFrom(des => des.Tags))
                .ReverseMap();

			CreateMap<Post, PostRequest>()
                .ForMember(src => src.Tags, config => config.MapFrom(des => des.Tags))
               .ReverseMap();

			CreateMap<ApplicationUser, RegisterRequest>()
				.ReverseMap();
		}
    }
}
