using Microsoft.Extensions.DependencyInjection;
using WeeControl.Core.Domain.Interfaces;
using WeeControl.Core.Test;

namespace WeeControl.Integration.Test;

public static class ExtensionMethods
{
    public static HttpClient CreateCustomClient<TStartup>(this CustomWebApplicationFactory<TStartup> factory)
        where TStartup : class
    {
        var client = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                using var scope = services.BuildServiceProvider().CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<IEssentialDbContext>();
                CoreTestHelper.SeedDatabase(db);
            });
        }).CreateClient();

        return client;
    }

    public static HttpClient CreateCustomClient<TStartup>(this CustomWebApplicationFactory<TStartup> factory,
        Action<IEssentialDbContext> essential) where TStartup : class
    {
        var client = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                using var scope = services.BuildServiceProvider().CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<IEssentialDbContext>();
                CoreTestHelper.SeedDatabase(db);
                essential.Invoke(db);
            });
        }).CreateClient();

        return client;
    }
}