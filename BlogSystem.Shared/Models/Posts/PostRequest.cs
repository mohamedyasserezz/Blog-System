namespace BlogSystem.Shared.Models.Posts
{
    public record PostRequest(
    string Title,
    string Content,
    ICollection<TagRequest> Tags
    );
}
