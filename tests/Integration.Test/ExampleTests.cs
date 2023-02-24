using Microsoft.Extensions.DependencyInjection;
using WeeControl.Core.Domain.Interfaces;
using WeeControl.Core.Test;
using WeeControl.Host.Test.ApiService;
using WeeControl.Host.WebApi;
using WeeControl.Host.WebApiService;

namespace WeeControl.Integration.Test;

public class ExampleTests : IClassFixture<CustomWebApplicationFactory<Startup>>
{
    private readonly CustomWebApplicationFactory<Startup> factory;

    public ExampleTests(CustomWebApplicationFactory<Startup> factory)
    {
        this.factory = factory;
    }

    [Fact]
    public async void TestAuthorizeInCustomerWebApplication()
    {
        using var hostTestHelper = new HostTestHelper();
        var service = hostTestHelper.GetService<ISecurity>(factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                using var scope = services.BuildServiceProvider().CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<IEssentialDbContext>();
                
                CoreTestHelper.SeedDatabase(db);
            });
        }).CreateClient());

        await factory.Authorize(hostTestHelper, CoreTestHelper.Username, CoreTestHelper.Password);
        
        Assert.True(await service.IsAuthenticated());
    }
}