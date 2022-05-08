using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WeeControl.Backend.Domain.Databases.Databases;
using WeeControl.Backend.Persistence.BoundedContext.Credentials;

namespace WeeControl.Backend.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistenceAsPostgreSql(this IServiceCollection services, IConfiguration configuration, string migrationAssemblyName)
        {
            services.AddScoped<IEssentialDbContext>(p =>
                new EssentialDbContext(GetPostgresOptions<EssentialDbContext>(configuration.GetConnectionString("HumanResourcesDbProvider"), migrationAssemblyName)));

            return services;
        }

        public static IServiceCollection AddPersistenceAsInMemory(this IServiceCollection services)
        {
            services.RemoveDbFromServices<EssentialDbContext>();
            services.AddScoped<IEssentialDbContext>(p =>
                new EssentialDbContext(GetInMemoryOptions<EssentialDbContext>(new Random().NextDouble().ToString())));

            return services;
        }

        static private DbContextOptions<T> GetPostgresOptions<T>(string dbName, string migrationAssemblyName) where T :DbContext
        {
            var options = new DbContextOptionsBuilder<T>();
#if DEBUG
            options.EnableSensitiveDataLogging();
#endif
            options.UseNpgsql(dbName, o =>
            {
                o.MigrationsAssembly(migrationAssemblyName);
            });

            return options.Options;
        }

        static private IServiceCollection RemoveDbFromServices<T>(this IServiceCollection services) where T: DbContext
        {
            var descriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(DbContextOptions<T>));
            if (descriptor != null)
                services.Remove(descriptor);
            return services;
        }

        static private DbContextOptions<T> GetInMemoryOptions<T>(string dbName) where T: DbContext
        {
            var options = new DbContextOptionsBuilder<T>();
            options.EnableDetailedErrors();
            options.EnableSensitiveDataLogging();
            options.UseInMemoryDatabase(dbName);
            options.ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));

            return options.Options;
        }
    }
}
