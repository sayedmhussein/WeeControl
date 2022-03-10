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
            services.AddDbContext<Credentials.CredentialsDbContext>(options => PostgresBuilder(configuration, migrationAssemblyName));

            //            services.AddDbContext<HumanResourcesDbContext>(options =>
            //            {
            //#if DEBUG
            //                options.EnableSensitiveDataLogging();
            //#endif
            //                options.UseNpgsql(configuration.GetConnectionString("HumanResourcesDbProvider"), o =>
            //                {
            //                    o.MigrationsAssembly(migrationAssemblyName);
            //                });
            //            });

            services.AddScopedContexts();

            return services;
        }

        public static IServiceCollection AddPersistenceAsInMemory(this IServiceCollection services)
        {
            services.RemoveDbFromServices<CredentialsDbContext>();
            services.AddDbContext<CredentialsDbContext>(options => InMemoryBuilder(nameof(CredentialsDbContext)));


            //services.AddDbContext<HumanResourcesDbContext>(options =>
            //{
            //    options.EnableSensitiveDataLogging();
            //    options.UseInMemoryDatabase(databaseName);
            //    options.ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            //});

            services.AddScopedContexts();

            return services;
        }

        private static IServiceCollection AddScopedContexts(this IServiceCollection services)
        {
            services.AddScoped<ICredentialsDbContext>(p => p.GetService<CredentialsDbContext>());
            return services;
        }

        static private DbContextOptionsBuilder PostgresBuilder(IConfiguration configuration, string migrationAssemblyName)
        {
            var options = new DbContextOptionsBuilder();
#if DEBUG
            options.EnableSensitiveDataLogging();
#endif
            options.UseNpgsql(configuration.GetConnectionString("HumanResourcesDbProvider"), o =>
            {
                o.MigrationsAssembly(migrationAssemblyName);
            });

            return options;
        }


        static private IServiceCollection RemoveDbFromServices<T>(this IServiceCollection services) where T: DbContext
        {
            var descriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(DbContextOptions<T>));

            services.Remove(descriptor);
            return services;
        }

        static private DbContextOptionsBuilder InMemoryBuilder(string dbName)
        {
            var options = new DbContextOptionsBuilder();
            options.EnableSensitiveDataLogging();
            options.UseInMemoryDatabase(dbName);
            options.ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));

            return options;
        }
    }
}
