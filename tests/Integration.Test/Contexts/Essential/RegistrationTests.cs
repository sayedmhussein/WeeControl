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
    public async void TestWhenSuccess()
    {
        
        using var hostTestHelper = new HostTestHelper(factory.CreateClient());
        
        var service = hostTestHelper.GetService<IUserService>();
        var dto = new UserProfileDto()
        {
            Person =
            {
                FirstName = "FirstName", LastName = "LastName", NationalityCode = "EGP", DateOfBirth = DateTime.Today
            },
            User = { Email = "email@email.com", Username = "username", Password = "Password"}
        };

        await service.AddUser(dto);
        await hostTestHelper.GetService<IAuthenticationService>().UpdateToken("0000");
        
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
        using var hostTestHelper = new HostTestHelper(factory.CreateCustomClient());

        var service = hostTestHelper.GetService<IUserService>();
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