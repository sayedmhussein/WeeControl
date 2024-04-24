using WeeControl.Core.DomainModel.Essentials.Dto;
using WeeControl.Core.Test;
using WeeControl.Host.Test.ApiService;
using WeeControl.Host.WebApi;
using WeeControl.Host.WebApiService.Contexts.Essentials;
using WeeControl.Host.WebApiService.Data;
using WeeControl.Host.WebApiService.DeviceInterfaces;

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

        var service = hostTestHelper.GetService<IPersonService>();
        var dto = new UserProfileDto
        {
            FirstName = "FirstName", LastName = "LastName", NationalityCode = "EGP", DateOfBirth = DateTime.Today,
            Email = "email@email.com", Username = "username", Password = "Password"
        };

        await service.Register(dto);
        await hostTestHelper.GetService<IAuthenticationService>().UpdateToken("0000");

        hostTestHelper.GuiMock.Verify(x =>
            x.NavigateTo(ApplicationPages.Essential.OtpPage, It.IsAny<bool>()), Times.Once);
        hostTestHelper.GuiMock.Verify(x =>
            x.NavigateTo(ApplicationPages.Essential.HomePage, It.IsAny<bool>()), Times.Once);
    }

    [Theory]
    [InlineData("SomeOtherEmail@email.com", CoreTestHelper.Username)]
    [InlineData(CoreTestHelper.Email, "SomeOtherUsername")]
    public async void TestWhenSameEmailOrUsername(string email, string username)
    {
        using var hostTestHelper = new HostTestHelper(factory.CreateCustomClient());

        var service = hostTestHelper.GetService<IPersonService>();
        var dto = new UserProfileDto
        {
            FirstName = "FirstName", LastName = "LastName", NationalityCode = "EGP", DateOfBirth = DateTime.Today,
            Email = email, Username = username, Password = "Password"
        };

        await service.Register(dto);

        hostTestHelper.GuiMock.Verify(x =>
            x.NavigateTo(ApplicationPages.Essential.OtpPage, It.IsAny<bool>()), Times.Never);
        hostTestHelper.GuiMock.Verify(x =>
            x.DisplayAlert(It.IsAny<string>(), It.IsAny<IGui.Severity>()), Times.Once);
    }
}