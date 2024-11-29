#nullable disable

namespace BlogSystem.Domain.Enities
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string AuthorId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Tag> Tags { get; set; } = new HashSet<Tag>();
        public string CategoryId { get; set; }

    }
}
