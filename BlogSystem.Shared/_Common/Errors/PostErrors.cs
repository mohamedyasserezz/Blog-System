using BlogSystem.Shared.Abstractions;

namespace BlogSystem.Shared.Common.Errors
{
    public static class PostErrors
    {
        public static Error PostNotFound =
        new("Post.NotFound", "No post was found with the given id");
    }
}
