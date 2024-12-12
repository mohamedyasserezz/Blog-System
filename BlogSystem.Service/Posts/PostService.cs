using BlogSystem.Core.Models;
using BlogSystem.Core.ServiceAbstraction.Posts;
using BlogSystem.Domain.Contract.Infrastructure;
using BlogSystem.Domain.Enities;

namespace BlogSystem.Service.Posts
{
    internal class PostService(IUnitOfWork unitOfWork, IMapper mapper) : IPostService
    {
        public Task<Post?> CreatePostAsync(string userEmail, PostToCreateDto postToCreateDto)
        {
            throw new NotImplementedException();
        }
    }
}
