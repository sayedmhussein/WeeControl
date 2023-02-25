using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Globalization;
using System.Linq;
using WeeControl.ApiApp.Persistence.DbContexts;
using WeeControl.Core.Domain.Interfaces;


namespace WeeControl.ApiApp.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services,
            string dbName, string migrationAssemblyName)
        {
            var options = GetOptions<EssentialDbContext>(
                dbName,
                migrationAssemblyName);

            services.AddScoped<IEssentialDbContext>(p =>
                new EssentialDbContext(options));

            return services;
        }

        public static IServiceCollection AddPersistenceAsInMemory(this IServiceCollection services,
            string dbName = null)
        {
            services.RemoveDbFromServices<EssentialDbContext>();

            dbName ??= new Random().NextDouble().ToString(CultureInfo.InvariantCulture);
            services.AddScoped<IEssentialDbContext>(p =>
                new EssentialDbContext(GetInMemoryOptions<EssentialDbContext>(dbName)));

            return services;
        }

        private static DbContextOptions<T> GetOptions<T>(string dbName, string migrationAssemblyName)
            where T : DbContext
        {
            var options = new DbContextOptionsBuilder<T>();
#if DEBUG
            options.EnableSensitiveDataLogging();
#endif
            //options.UseNpgsql(dbName, b => b.MigrationsAssembly(migrationAssemblyName));

            options.EnableDetailedErrors();

            options.UseMySQL(dbName, b =>
            {
                b.MigrationsAssembly(migrationAssemblyName);
                //b.SchemaBehavior(MySqlSchemaBehavior.Translate, (schema, table) => $"{schema}_{table}");
            });
            
            return options.Options;
        }

        private static IServiceCollection RemoveDbFromServices<T>(this IServiceCollection services) where T : DbContext
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                     typeof(DbContextOptions<T>));
            if (descriptor != null)
                services.Remove(descriptor);
            return services;
        }

        private static DbContextOptions<T> GetInMemoryOptions<T>(string dbName) where T : DbContext
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