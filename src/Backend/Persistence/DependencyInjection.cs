using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WeeControl.Backend.Domain.BoundedContexts.HumanResources;
using WeeControl.Backend.Persistence.BoundedContexts.HumanResources;

namespace WeeControl.Backend.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistenceAsPostgreSql(this IServiceCollection services, IConfiguration configuration, string migrationAssemblyName)
        {
            services.AddDbContext<HumanResourcesDbContext>(options =>
            {
#if DEBUG
                options.EnableSensitiveDataLogging();
#endif
                options.UseNpgsql(configuration.GetConnectionString("HumanResourcesDbProvider"), o =>
                {
                    o.MigrationsAssembly(migrationAssemblyName);
                });
            });

            services.AddScopedContexts();

            return services;
        }

        public static IServiceCollection AddPersistenceAsInMemory(this IServiceCollection services, string databaseName)
        {
            services.AddDbContext<HumanResourcesDbContext>(options =>
            {
                options.EnableSensitiveDataLogging();
                options.UseInMemoryDatabase(databaseName);
                options.ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            });

            services.AddScopedContexts();

            return services;
        }

        private static IServiceCollection AddScopedContexts(this IServiceCollection services)
        {
            services.AddScoped<IHumanResourcesDbContext>(p => p.GetService<HumanResourcesDbContext>());
            return services;
        }
    }
}
