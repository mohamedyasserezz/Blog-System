﻿using BlogSystem.Domain.Contract.Posts;
using BlogSystem.Service.Mapping;
using BlogSystem.Service.Posts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlogSystem.Service
{
    public static class DependectInjection
    {
        public static IServiceCollection AddServicesDependency(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IPostService, PostService>();
            services.AddAutoMapper(typeof(MappingProfile));

            return services;
        }
    }
}
