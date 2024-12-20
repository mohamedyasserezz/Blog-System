namespace BlogSystem.Shared.Models.Posts
{
    public record PostToCreateDto(
    string Title,
    string Content,
    string AuthorId
    );
}
