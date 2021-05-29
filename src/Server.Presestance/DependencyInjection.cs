using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySystem.Application.Common.Interfaces;

namespace MySystem.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<MySystemDbContext>(options =>
            {
                options.EnableSensitiveDataLogging();
                options.UseNpgsql(configuration.GetConnectionString("DbConnection"));
            });

            services.AddScoped<IMySystemDbContext>(provider => provider.GetService<MySystemDbContext>());

            return services;
        }

        public static IServiceCollection AddPersistenceAsInMemory(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<MySystemDbContext>(options =>
            {
                options.EnableSensitiveDataLogging();
                options.UseInMemoryDatabase(configuration.GetConnectionString("DbConnection"));
                options.ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            });

            services.AddScoped<IMySystemDbContext>(provider => provider.GetService<MySystemDbContext>());

            return services;
        }
    }
}
