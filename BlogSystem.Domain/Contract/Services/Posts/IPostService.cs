using BlogSystem.Domain.Enities;
using BlogSystem.Shared.Models.Posts;

namespace BlogSystem.Domain.Contract.Posts
{
    public interface IPostService
    {
        Task<Post?> CreatePostAsync(string userEmail, PostToCreateDto postToCreateDto, CancellationToken cancellationToken = default);
    }
}
