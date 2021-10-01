using System;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using WeeControl.Server.Domain.Authorization;
using WeeControl.Server.Domain.HumanResources;
using WeeControl.Server.Persistence;
using WeeControl.Server.Persistence.HumanResources;
using Xunit;

[assembly: CollectionBehavior(CollectionBehavior.CollectionPerAssembly, DisableTestParallelization = true, MaxParallelThreads = 1)]

namespace WeeControl.Server.Test.Functional.Common
{
    public class CustomWebApplicationFactory<TStartup>
    : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                RemovePhysicalDatabaseContexts(services);

                services.AddPersistenceAsInMemoryDb();

                var sp = services.BuildServiceProvider();

                using var scope = sp.CreateScope();
                ConfigureDatabases(scope.ServiceProvider);
            });
        }

        private static void RemovePhysicalDatabaseContexts(IServiceCollection services)
        {
            var desc = services.FirstOrDefault(d => d.ServiceType == typeof(IAuthorizationDbContext));
            services.Remove(desc);
        }

        private void ConfigureDatabases(IServiceProvider provider)
        {
            var db = (HumanResourcesDbContext)provider.GetRequiredService<IHumanResourcesDbContext>();

            db.Database.EnsureCreated();
        }
    }
}
