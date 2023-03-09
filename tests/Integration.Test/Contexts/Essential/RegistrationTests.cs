using Microsoft.Extensions.DependencyInjection;
using WeeControl.Core.DataTransferObject.Contexts.Essentials;
using WeeControl.Core.Domain.Interfaces;
using WeeControl.Core.Test;
using WeeControl.Host.Test.ApiService;
using WeeControl.Host.WebApi;
using WeeControl.Host.WebApiService;
using WeeControl.Host.WebApiService.Contexts.Essentials;

namespace WeeControl.Integration.Test.Contexts.Essential;

public class RegistrationTests : IClassFixture<CustomWebApplicationFactory<Startup>>
{
    private readonly CustomWebApplicationFactory<Startup> factory;

    public RegistrationTests(CustomWebApplicationFactory<Startup> factory)
    {
        this.factory = factory;
    }

    [Fact]
    public async void TestWhenSuccess()
    {
        using var hostTestHelper = new HostTestHelper();
        var client = factory.CreateClient();
        var service = hostTestHelper.GetService<IUserService>(client);
        var dto = new UserProfileDto()
        {
            Person =
            {
                FirstName = "FirstName", LastName = "LastName", NationalityCode = "EGP", DateOfBirth = DateTime.Today
            },
            User = { Email = "email@email.com", Username = "username", Password = "Password"}
        };

        await service.AddUser(dto);
        await hostTestHelper.GetService<IAuthenticationService>(client).UpdateToken("0000");
        
        hostTestHelper.GuiMock.Verify(x => 
            x.NavigateToAsync(ApplicationPages.Essential.OtpPage, It.IsAny<bool>()), Times.Once);
        hostTestHelper.GuiMock.Verify(x => 
                x.NavigateToAsync(ApplicationPages.Essential.HomePage, It.IsAny<bool>()), Times.Once);
    }
    
    [Theory]
    [InlineData("SomeOtherEmail@email.com", CoreTestHelper.Username)]
    [InlineData(CoreTestHelper.Email, "SomeOtherUsername")]
    public async void TestWhenSameEmailOrUsername(string email, string username)
    {
        using var hostTestHelper = new HostTestHelper();
        var client = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                using var scope = services.BuildServiceProvider().CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<IEssentialDbContext>();
                CoreTestHelper.SeedDatabase(db);
            });
        }).CreateClient();
        
        var service = hostTestHelper.GetService<IUserService>(factory.CreateCustomClient(factory));
        var dto = new UserProfileDto()
        {
            Person =
            {
                FirstName = "FirstName", LastName = "LastName", NationalityCode = "EGP", DateOfBirth = DateTime.Today
            },
            User = { Email = email, Username = username, Password = "Password"}
        };

        await service.AddUser(dto);

        hostTestHelper.GuiMock.Verify(x => 
            x.NavigateToAsync(ApplicationPages.Essential.OtpPage, It.IsAny<bool>()), Times.Never);
        hostTestHelper.GuiMock.Verify(x => 
            x.DisplayAlert(It.IsAny<string>()), Times.Once);
    }
}