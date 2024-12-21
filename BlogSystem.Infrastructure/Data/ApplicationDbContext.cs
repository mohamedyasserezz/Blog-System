using BlogSystem.Domain.Enities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Security.Claims;

namespace BlogSystem.Infrastructure.Data
{
	public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContext, IHttpContextAccessor httpContextAccessor) : IdentityDbContext<ApplicationUser, ApplicationRole, string>(dbContext)
	{
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public DbSet<ApplicationUser> Users { get; set; }
		public DbSet<Comment> Categories { get; set; }
		public DbSet<Comment> Comments { get; set; }
		public DbSet<Post> Posts { get; set; }
		public DbSet<Tag> Tags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
		}

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries<Post>();

            foreach (var entry in entries)
            {
                var currentUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (entry.State == EntityState.Added)
                {
                    entry.Property(X => X.AuthorId).CurrentValue = currentUserId!;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
