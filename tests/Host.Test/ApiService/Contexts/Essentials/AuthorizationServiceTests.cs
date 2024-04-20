using System.Net;
using WeeControl.Core.DomainModel.Essentials.Dto;
using WeeControl.Host.WebApiService.Contexts.Essentials;
using WeeControl.Host.WebApiService.Data;
using WeeControl.Host.WebApiService.DeviceInterfaces;
using WeeControl.Host.WebApiService.Internals.Interfaces;

namespace WeeControl.Host.Test.ApiService.Contexts.Essentials;

public class AuthorizationServiceTests
{
    #region Logout()

    [Theory]
    [InlineData(HttpStatusCode.OK)]
    [InlineData(HttpStatusCode.BadRequest)]
    public async void Logout_WhenNotFound_OrSuccess(HttpStatusCode httpStatusCode)
    {
        using var helper = new HostTestHelper(httpStatusCode);
        var service = helper.GetService<IAuthenticationService>();

        await service.Logout();

        helper.GuiMock
            .Verify(x =>
                x.NavigateTo(ApplicationPages.Essential.LoginPage, It.IsAny<bool>()), Times.Once);
    }

    #endregion

    #region LoginTests

    [Fact]
    public async void WhenSuccessUsernameAndPasswordAndOtp()
    {
        using var helper = new HostTestHelper(new (HttpStatusCode statusCode, object? dto)[]
        {
            (HttpStatusCode.OK, TokenResponseDto.Create("token")),
            (HttpStatusCode.OK, TokenResponseDto.Create("token")),
            (HttpStatusCode.OK, TokenResponseDto.Create("token"))
        });
        var service = helper.GetService<IAuthenticationService>();

        await service.Login(LoginRequestDto.Create("username", "password"));
        await service.UpdateToken("0000");

        helper.GuiMock.Verify(x =>
            x.NavigateTo(ApplicationPages.Essential.HomePage, It.IsAny<bool>()), Times.Once);
    }

    [Theory]
    [InlineData(HttpStatusCode.NotFound)]
    [InlineData(HttpStatusCode.BadRequest)]
    [InlineData(HttpStatusCode.BadGateway)]
    [InlineData(HttpStatusCode.InternalServerError)]
    public async void Login_ExpectedResponsesBehaviorTests(HttpStatusCode code)
    {
        using var helper = new HostTestHelper(new (HttpStatusCode statusCode, object? dto)[]
        {
            new(code, TokenResponseDto.Create("token")),
            new(code, TokenResponseDto.Create("token")),
            new(code, TokenResponseDto.Create("token"))
        });
        var service = helper.GetService<IAuthenticationService>();

        await service.Login(LoginRequestDto.Create("username", "password"));

        helper.GuiMock.Verify(x =>
            x.DisplayAlert(It.IsAny<string>(), It.IsAny<IGui.Severity>()), Times.Once);
        helper.GuiMock.Verify(x =>
            x.NavigateTo(ApplicationPages.Essential.HomePage, true), Times.Never);
    }

    [Theory]
    [InlineData("", "")]
    [InlineData("", "password")]
    [InlineData("username", "")]
    [InlineData("    ", "password")]
    [InlineData("username", "    ")]
    [InlineData("   ", "    ")]
    public async void Login_DataTransferObjectDefensiveValues(string username, string password)
    {
        using var helper = new HostTestHelper(HttpStatusCode.OK, TokenResponseDto.Create("token"));
        var service = helper.GetService<IAuthenticationService>();

        await service.Login(LoginRequestDto.Create(username, password));

        helper.GuiMock.Verify(x =>
            x.DisplayAlert(It.IsAny<string>(), It.IsAny<IGui.Severity>()), Times.Once);
        helper.GuiMock.Verify(x =>
            x.NavigateTo(ApplicationPages.Essential.HomePage, true), Times.Never);
    }

    #endregion

    #region UpdateToken()

    [Theory]
    [InlineData(HttpStatusCode.OK)]
    [InlineData(HttpStatusCode.Unauthorized)]
    [InlineData(HttpStatusCode.NotFound)]
    [InlineData(HttpStatusCode.BadRequest)]
    [InlineData(HttpStatusCode.BadGateway)]
    [InlineData(HttpStatusCode.InternalServerError)]
    public async void UpdateToken_ExpectedResponsesBehaviorTests(HttpStatusCode code)
    {
        using var helper = new HostTestHelper(code, TokenResponseDto.Create("token"));
        var service = helper.GetService<IAuthenticationService>();
        helper.StorageMock.Setup(x => x.GetKeyValue(IDeviceSecurity.TokenKeyName))
            .ReturnsAsync("value");

        await service.UpdateToken("value");

        helper.GuiMock.Verify(x =>
                x.DisplayAlert(It.IsAny<string>(), It.IsAny<IGui.Severity>()),
            code == HttpStatusCode.OK ? Times.Never : Times.Once);

        helper.GuiMock.Verify(x =>
                x.NavigateTo(ApplicationPages.Essential.HomePage, It.IsAny<bool>()),
            code == HttpStatusCode.OK ? Times.Once : Times.Never);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("1")]
    [InlineData("12")]
    [InlineData("123")]
    public async void UpdateToken_DataTransferObjectDefensiveValues(string value)
    {
        using var helper = new HostTestHelper(HttpStatusCode.OK, TokenResponseDto.Create("token"));
        var service = helper.GetService<IAuthenticationService>();

        await service.UpdateToken(value);

        helper.GuiMock.Verify(x =>
            x.DisplayAlert(It.IsAny<string>(), It.IsAny<IGui.Severity>()), Times.AtLeastOnce);

        helper.GuiMock.Verify(x =>
            x.NavigateTo(ApplicationPages.Essential.HomePage, It.IsAny<bool>()), Times.Never);
    }

    #endregion
}