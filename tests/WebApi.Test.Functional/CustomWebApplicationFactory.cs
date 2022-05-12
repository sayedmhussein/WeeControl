﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using WeeControl.Backend.Persistence;
using Xunit;

[assembly: CollectionBehavior(CollectionBehavior.CollectionPerAssembly, DisableTestParallelization = true, MaxParallelThreads = 1)]

namespace WeeControl.test.WebApi.Test.Functional
{
    public class CustomWebApplicationFactory<TStartup>
        : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.AddPersistenceAsInMemory();
                // var sp = services.BuildServiceProvider();
                // using var scope = sp.CreateScope();
                
            });
        }
    }
}