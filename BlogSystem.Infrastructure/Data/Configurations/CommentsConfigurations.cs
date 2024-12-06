using BlogSystem.Domain.Enities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogSystem.Infrastructure.Data.Configurations
{
    public class CommentsConfigurations : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasKey(C => C.Id);
            builder.Property(C => C.PostId).IsRequired();
            builder.Property(C => C.AuthorId).IsRequired();
        }
    }
}
