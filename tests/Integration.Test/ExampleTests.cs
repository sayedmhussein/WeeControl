using WeeControl.Host.Test.ApiService;
using WeeControl.Host.WebApi;
using WeeControl.Host.WebApiService.Interfaces;

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
        using var testHelper = new HostTestHelper(factory.CreateCustomClient());
        await testHelper.Authenticate();

        var service = testHelper.GetService<ISecurity>();

        Assert.True(await service.IsAuthenticated());
    }
}