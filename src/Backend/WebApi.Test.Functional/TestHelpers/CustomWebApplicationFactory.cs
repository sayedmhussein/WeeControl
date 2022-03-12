using System;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using WeeControl.Backend.Domain.Credentials;
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
                //services.AddDbContext<CredentialsDbContext>(options =>
                //{
                //    options.EnableSensitiveDataLogging();
                //    options.UseInMemoryDatabase(nameof(CredentialsDbContext));
                //    options.ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));
                //});
                //services.AddScoped<ICredentialsDbContext, CredentialsDbContext>();

                //services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

                services.AddPersistenceAsInMemory();


                //var sp = services.BuildServiceProvider();

                //using var scope = sp.CreateScope();
                //var db = (CredentialsDbContext)scope.ServiceProvider.GetRequiredService<ICredentialsDbContext>();
                //db.Database.EnsureCreated();

                //var sp = services.BuildServiceProvider();

                //using var scope = sp.CreateScope();
                //var scopedServices = scope.ServiceProvider;
                //var db = scopedServices.GetRequiredService<CredentialsDbContext>();

                //db.Database.EnsureCreated();
            });
        }
    }
}
