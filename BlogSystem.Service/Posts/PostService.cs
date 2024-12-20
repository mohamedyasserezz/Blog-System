using AutoMapper;
using BlogSystem.Domain.Contract.Infrastructure;
using BlogSystem.Domain.Contract.Posts;
using BlogSystem.Domain.Enities;
using BlogSystem.Shared.Models.Posts;
using Microsoft.AspNetCore.Identity;

namespace BlogSystem.Service.Posts
{
    internal class PostService(
        IUnitOfWork unitOfWork,
        UserManager<ApplicationUser> userManager,
        IMapper mapper
        ) : IPostService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IMapper _mapper = mapper;

        public async Task<Post?> CreatePostAsync(string userEmail, PostToCreateDto postToCreateDto, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByEmailAsync(userEmail);

            if (user is null) return null;


            var post = _mapper.Map<Post>(postToCreateDto);

            
            if (post is null) return null;

            await _unitOfWork.GetRepository<Post>().AddAsync(post);

            var created = await unitOfWork.CompleteAsync() > 0;

            if (!created) return null;  

            return post;
        }
    }
}
