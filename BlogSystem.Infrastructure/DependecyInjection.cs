using BlogSystem.Domain.Contract.Infrastructure;
using BlogSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlogSystem.Infrastructure
{
    public static class DependecyInjection
    {
        public static IServiceCollection AddPersistanceServices(this IServiceCollection services,
       IConfiguration configuration)
        {
            #region StoreContext

            services.AddDbContext<ApplicationDbContext>((optionsBuilder) =>
            {
                optionsBuilder.UseSqlServer(configuration.GetConnectionString("ApplicationDbContext"));
            });


            #endregion

            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork.UnitOfWork));

            return services;
        }
    }
}
