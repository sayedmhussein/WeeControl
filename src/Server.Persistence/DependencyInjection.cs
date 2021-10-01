using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WeeControl.Server.Domain.Administration;
using WeeControl.Server.Domain.Authorization;
using WeeControl.Server.Domain.HumanResources;
using WeeControl.Server.Persistence.Administration;
using WeeControl.Server.Persistence.Authorization;
using WeeControl.Server.Persistence.HumanResources;

namespace WeeControl.Server.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistenceAsPostgreSql(this IServiceCollection services, IConfiguration configuration, string migrationAssemblyName)
        {
            services.AddDbContext<AdministrationDbContext>((opt) => new DbContextOptionsBuilder(GetOptions("AdministrationDbProvider", migrationAssemblyName)));
            services.AddDbContext<AuthorizationDbContext>(opt => new DbContextOptionsBuilder(GetOptions("AuthorizationDbProvider", migrationAssemblyName)));
            
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

        [Obsolete]
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
            services.AddScoped<IAdministrationDbContext>(p => p.GetService<AdministrationDbContext>());
            services.AddScoped<IAuthorizationDbContext>(p => p.GetService<AuthorizationDbContext>());
            services.AddScoped<IHumanResourcesDbContext>(p => p.GetService<HumanResourcesDbContext>());
            return services;
        }

        private static DbContextOptions GetOptions(string connectionString, string migrationAssemblyName)
        {
            DbContextOptionsBuilder options = new();
#if DEBUG
            options.EnableSensitiveDataLogging();
#endif
            options.UseNpgsql(connectionString, o =>
            {
                o.MigrationsAssembly(migrationAssemblyName);
            });
            
            

            return options.Options;
        }
    }
}
