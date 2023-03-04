using WeeControl.Core.DataTransferObject.Contexts.Essentials;
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
    public async void Test()
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
}