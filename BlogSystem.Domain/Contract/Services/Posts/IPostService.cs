﻿using BlogSystem.Shared.Abstractions;
using BlogSystem.Shared.Models.Common;
using BlogSystem.Shared.Models.Posts;

namespace BlogSystem.Domain.Contract.Posts
{
    public interface IPostService
    {
        Task<Result<Pagination<PostResponse>>> GetAllAsync(RequestFilter requestFilter, CancellationToken cancellationToken = default);
        Task<Result<PostResponse>> GetAsync(int id, CancellationToken cancellationToken = default);
        Task<Result<PostResponse>> CreatePostAsync(PostRequest postRequest, CancellationToken cancellationToken = default);
        Task<Result> UpdateAsync(int id, PostRequest poll, CancellationToken cancellationToken = default);
        Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
