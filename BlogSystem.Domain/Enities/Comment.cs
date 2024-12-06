
namespace BlogSystem.Domain.Enities
{
    public class Comment
    {
        public int Id { get; set; }
        public string? Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string PostId { get; set; } = null!;
        public string AuthorId { get; set; } = null!;
    }
}
