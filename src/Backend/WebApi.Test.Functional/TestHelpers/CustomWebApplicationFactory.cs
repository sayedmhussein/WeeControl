using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using WeeControl.Backend.Persistence;
using WeeControl.Backend.Persistence.Credentials;
using Xunit;

[assembly: CollectionBehavior(CollectionBehavior.CollectionPerAssembly, DisableTestParallelization = true, MaxParallelThreads = 1)]

namespace WeeControl.Backend.WebApi.Test.Functional.TestHelpers
{
    public class CustomWebApplicationFactory<TStartup>
    : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.AddPersistenceAsInMemory();

                //var sp = services.BuildServiceProvider();

                //using var scope = sp.CreateScope();
                //var scopedServices = scope.ServiceProvider;
                //var db = scopedServices.GetRequiredService<HumanResourcesDbContext>();

                //db.Database.EnsureCreated();
            });
        }
    }
}
