using BlogSystem.Core.Models;
using BlogSystem.Domain.Enities;

namespace BlogSystem.Core.ServiceAbstraction.Posts
{
    public interface IPostService
    {
        Task<Post?> CreatePostAsync(string userEmail, PostToCreateDto postToCreateDto);
    }
}
