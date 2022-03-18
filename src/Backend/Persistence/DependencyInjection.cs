using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WeeControl.Backend.Domain.Credentials;
using WeeControl.Backend.Persistence.Credentials;

namespace WeeControl.Backend.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistenceAsPostgreSql(this IServiceCollection services, IConfiguration configuration, string migrationAssemblyName)
        {
            services.AddScoped<ICredentialsDbContext>(p =>
                new CredentialsDbContext(GetPostgresOptions<CredentialsDbContext>(configuration.GetConnectionString("HumanResourcesDbProvider"), migrationAssemblyName)));

            return services;
        }

        public static IServiceCollection AddPersistenceAsInMemory(this IServiceCollection services)
        {
            //services.RemoveDbFromServices<CredentialsDbContext>();
            services.AddScoped<ICredentialsDbContext>(p =>
                new CredentialsDbContext(GetInMemoryOptions<CredentialsDbContext>(new Random().NextDouble().ToString())));

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
