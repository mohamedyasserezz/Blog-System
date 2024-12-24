using AutoMapper;
using AutoMapper.QueryableExtensions;
using BlogSystem.Domain.Contract.Infrastructure;
using BlogSystem.Domain.Contract.Posts;
using BlogSystem.Domain.Enities;
using BlogSystem.Infrastructure.Data;
using BlogSystem.Shared.Abstractions;
using BlogSystem.Shared.Common.Errors;
using BlogSystem.Shared.Models.Common;
using BlogSystem.Shared.Models.Posts;
using Microsoft.EntityFrameworkCore;

namespace BlogSystem.Service.Posts
{
    internal class PostService(
        IUnitOfWork unitOfWork,
        ApplicationDbContext applicationDbContext,
        IMapper mapper
        ) : IPostService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ApplicationDbContext _applicationDbContext = applicationDbContext;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<Pagination<PostResponse>>> GetAllAsync(RequestFilter requestFilter,CancellationToken cancellationToken = default)
        {
            var source = _applicationDbContext.Posts
                        .Include(x => x.Tags)
                        .ProjectTo<PostResponse>(_mapper.ConfigurationProvider)
                        .AsNoTracking();
            var posts = await Pagination<PostResponse>.CreateAsync(source, requestFilter.PageNumber, requestFilter.PageSize, cancellationToken);

            return Result.Success(posts);
        }

        public async Task<Result<PostResponse>> GetAsync(int id, CancellationToken cancellationToken = default)
        {
            var post = await _unitOfWork.GetRepository<Post>().GetByIdAsync(id, cancellationToken);

            if (post is null)
                return Result.Failer<PostResponse>(PostErrors.PostNotFound);

            var postResponse = _mapper.Map<PostResponse>(post);

            return Result.Success(postResponse);
        }
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
        public async Task<Result> UpdateAsync(int id, PostRequest postRequest, CancellationToken cancellationToken = default)
        {
            var Current = await _unitOfWork.GetRepository<Post>().GetByIdAsync(id, cancellationToken);

            if (Current is null)
                return Result.Failer(PostErrors.PostNotFound);

            Current.Content = postRequest.Content;
            Current.Title = postRequest.Title;
            Current.Tags = _mapper.Map<ICollection<Tag>>(postRequest.Tags);
            _unitOfWork.GetRepository<Post>().Update(Current);

            var isUpdates = await unitOfWork.CompleteAsync() > 0;

            if (!isUpdates)
                return Result.Failer(PostErrors.PostNotFound);

            return Result.Success();
        }
        public async Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var psot = await _unitOfWork.GetRepository<Post>().GetByIdAsync(id, cancellationToken);
            if (psot is null)
                return Result.Failer(PostErrors.PostNotFound);

            _unitOfWork.GetRepository<Post>().Delete(psot);
            var isDeleted = await unitOfWork.CompleteAsync() > 0;

            if (!isDeleted)
                return Result.Failer(PostErrors.PostNotFound);

            return Result.Success();

        }


    }
}
