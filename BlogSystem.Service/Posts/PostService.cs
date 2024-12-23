using AutoMapper;
using BlogSystem.Domain.Contract.Infrastructure;
using BlogSystem.Domain.Contract.Posts;
using BlogSystem.Domain.Enities;
using BlogSystem.Shared.Abstractions;
using BlogSystem.Shared.Common.Errors;
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

        public async Task<Result<PostResponse>> CreatePostAsync(PostRequest postRequest, CancellationToken cancellationToken = default)
        {

            var post = _mapper.Map<Post>(postRequest);

            if (post is null) return Result.Failer<PostResponse>(PostErrors.PostNotFound);

            await _unitOfWork.GetRepository<Post>().AddAsync(post);

            var created = await unitOfWork.CompleteAsync() > 0;

            if (!created) return Result.Failer<PostResponse>(PostErrors.PostNotFound);

            var postResponse = _mapper.Map<PostResponse>(post);

            return Result.Success(postResponse);
        }

        public async Task<Result<PostResponse>> GetAsync(int id, CancellationToken cancellationToken = default)
        {
            var post = await _unitOfWork.GetRepository<Post>().GetByIdAsync(id);

            if(post is null)
                return Result.Failer<PostResponse>(PostErrors.PostNotFound);

            var postResponse = _mapper.Map<PostResponse>(post);

            return Result.Success(postResponse);
        }
    }
}
