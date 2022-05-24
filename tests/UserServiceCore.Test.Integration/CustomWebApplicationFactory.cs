using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using WeeControl.Persistence;
using WeeControl.User.UserServiceCore;
using WeeControl.User.UserServiceCore.Interfaces;

//[assembly: CollectionBehavior(CollectionBehavior.CollectionPerAssembly, DisableTestParallelization = true, MaxParallelThreads = 1)]

namespace WeeControl.User.ServiceCore.Test.Integration;

public class CustomWebApplicationFactory<TStartup>
    : WebApplicationFactory<TStartup> where TStartup : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.AddPersistenceAsInMemory();
            //DependencyExtension.AddUserServiceCore(services);
        });
    }
    
    
}