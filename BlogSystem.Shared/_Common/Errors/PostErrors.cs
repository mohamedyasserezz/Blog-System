using BlogSystem.Shared.Abstractions;
using Microsoft.AspNetCore.Http;

namespace BlogSystem.Shared.Common.Errors
{
    public static class PostErrors
    {
        public static Error PostNotFound =
        new("Post.NotFound", "No post was found with the given id", StatusCodes.Status404NotFound);
    }
}
