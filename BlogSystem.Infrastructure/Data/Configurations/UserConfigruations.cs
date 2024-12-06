using BlogSystem.Domain.Enities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogSystem.Infrastructure.Data.Configurations
{
    public class UserConfigruations : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.Property(U => U.FirstName).IsRequired();
            builder.Property(U => U.LastName).IsRequired();
        }
    }
}
