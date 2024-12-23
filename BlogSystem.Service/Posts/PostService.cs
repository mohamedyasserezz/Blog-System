using AutoMapper;
using BlogSystem.Domain.Contract.Infrastructure;
using BlogSystem.Domain.Contract.Posts;
using BlogSystem.Domain.Enities;
using BlogSystem.Shared.Abstractions;
using BlogSystem.Shared.Common.Errors;
using BlogSystem.Shared.Models.Posts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace BlogSystem.Service.Posts
{
    internal class PostService(
        IUnitOfWork unitOfWork,
        IMapper mapper
        ) : IPostService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<PostResponse>> CreatePostAsync(PostRequest postRequest, CancellationToken cancellationToken = default)
        {

            var post = _mapper.Map<Post>(postRequest);

            if (post is null) return Result.Failer<PostResponse>(PostErrors.PostNotFound);

            await _unitOfWork.GetRepository<Post>().AddAsync(post, cancellationToken);

            var created = await unitOfWork.CompleteAsync() > 0;

            if (!created) return Result.Failer<PostResponse>(PostErrors.PostNotFound);

            var postResponse = _mapper.Map<PostResponse>(post);

            return Result.Success(postResponse);
        }

        public async Task<Result<PostResponse>> GetAsync(int id, CancellationToken cancellationToken = default)
        {
            var post = await _unitOfWork.GetRepository<Post>().GetByIdAsync(id, cancellationToken);

            if (post is null)
                return Result.Failer<PostResponse>(PostErrors.PostNotFound);

            var postResponse = _mapper.Map<PostResponse>(post);

            return Result.Success(postResponse);
        }

        public async Task<Result> UpdateAsync(int id, PostRequest postRequest, CancellationToken cancellationToken = default)
        {
            var Current = await _unitOfWork.GetRepository<Post>().GetByIdAsync(id, cancellationToken);

            if (Current is null)
                return Result.Failer(PostErrors.PostNotFound);

            var post = _mapper.Map<Post>(postRequest);

            _unitOfWork.GetRepository<Post>().Update(post);

            var updates = await unitOfWork.CompleteAsync() > 0;

            if (!updates)
                return Result.Failer(PostErrors.PostNotFound);

            return Result.Success();
        }
    }
}
