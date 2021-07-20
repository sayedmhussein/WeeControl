using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WeeControl.Server.Domain.Interfaces;

namespace WeeControl.Server.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistenceAsPostgreSQL(this IServiceCollection services, IConfiguration configuration, string migrationAssemblyName)
        {
            services.AddEntityFrameworkNpgsql()
                .AddDbContextPool<MySystemDbContext>(options =>
            {
                options.EnableSensitiveDataLogging();
                options.UseNpgsql(configuration.GetConnectionString("DbConnection"), b =>
                {
                    b.MigrationsAssembly(migrationAssemblyName);
                });
            });

            services.AddScoped<IMySystemDbContext>(provider => provider.GetService<MySystemDbContext>());

            return services;
        }

        public static IServiceCollection AddPersistenceAsInMemory(this IServiceCollection services, string databaseName)
        {
            services.AddDbContext<MySystemDbContext>(options =>
            {
                options.EnableSensitiveDataLogging();
                options.UseInMemoryDatabase(databaseName ?? "InMemoryDatabase");
                options.ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            });

            services.AddScoped<IMySystemDbContext>(provider => provider.GetService<MySystemDbContext>());

            return services;
        }
    }
}
