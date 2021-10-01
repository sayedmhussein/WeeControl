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
            services.AddScoped<IAdministrationDbContext>(p => 
                new AdministrationDbContext(GetOptions<AdministrationDbContext>(configuration.GetConnectionString("AdministrationDbProvider"), migrationAssemblyName)));

            services.AddScoped<IAuthorizationDbContext>(p => 
                new AuthorizationDbContext(GetOptions<AuthorizationDbContext>(configuration.GetConnectionString("AuthorizationDbProvider"), migrationAssemblyName)));
            
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

            services.AddScoped<IHumanResourcesDbContext>(p => p.GetService<HumanResourcesDbContext>());

            return services;
        }
        
        private static DbContextOptions<T> GetOptions<T>(string connectionString, string migrationAssemblyName) where T : DbContext
        {
            DbContextOptionsBuilder<T> options = new();
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
