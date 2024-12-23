using BlogSystem.Domain.Contract.Posts;
using BlogSystem.Domain.Contract.Services.Authentication;
using BlogSystem.Service.Authentication;
using BlogSystem.Service.Mapping;
using BlogSystem.Service.Posts;
using Hangfire;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlogSystem.Service
{
    public static class DependectInjection
    {
        public static IServiceCollection AddServicesDependency(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IEmailSender, EmailService>();

            services.AddAutoMapper(typeof(MappingProfile));

			services.AddHangfire(config => config
					.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
					.UseSimpleAssemblyNameTypeSerializer()
					.UseRecommendedSerializerSettings()
					.UseSqlServerStorage(configuration.GetConnectionString("HangfireConnection")));

			services.AddHangfireServer();

			return services;
        }
    }
}
