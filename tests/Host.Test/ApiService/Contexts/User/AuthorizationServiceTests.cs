using System.Net;
using WeeControl.Core.DataTransferObject.Contexts.User;
using WeeControl.Host.WebApiService;
using WeeControl.Host.WebApiService.Contexts.User;
using WeeControl.Host.WebApiService.Internals.Interfaces;

namespace WeeControl.Host.Test.ApiService.Contexts.User;

public class AuthorizationServiceTests
{
    #region LoginTests
    
    [Fact]
    public async void WhenSuccessUsernameAndPasswordAndOtp()
    {
        using var helper = new HostTestHelper();
        var service = helper.GetService<IAuthenticationService>(
            new (HttpStatusCode statusCode, object? dto)[]
        {
            (HttpStatusCode.OK, TokenResponseDto.Create("token")),
            (HttpStatusCode.OK, TokenResponseDto.Create("token")),
            (HttpStatusCode.OK, TokenResponseDto.Create("token"))
        });

        await service.Login(LoginRequestDto.Create("username", "password"));
        await service.UpdateToken("0000");
        
        helper.GuiMock.Verify(x =>
            x.NavigateToAsync(ApplicationPages.Essential.HomePage, It.IsAny<bool>()), Times.Once);
    }

    [Theory]
    [InlineData(HttpStatusCode.OK, true)]
    [InlineData(HttpStatusCode.NotFound, false)]
    [InlineData(HttpStatusCode.BadRequest, false)]
    [InlineData(HttpStatusCode.BadGateway, false)]
    [InlineData(HttpStatusCode.InternalServerError, false)]
    public async void Login_ExpectedResponsesBehaviorTests(HttpStatusCode code, bool fnReturn)
    {
        using var helper = new HostTestHelper();

        var expected = new (HttpStatusCode statusCode, object? dto)[]
        {
            new (code, TokenResponseDto.Create("token")),
            new (code, TokenResponseDto.Create("token")),
            new (code, TokenResponseDto.Create("token"))
        };

        var service = helper.GetService<IAuthenticationService>(expected);

        await service.Login(LoginRequestDto.Create("username", "password"));

        switch (code)
        {
            case HttpStatusCode.OK:
                // helper.DeviceMock.Verify(x => 
                //     x.DisplayAlert(It.IsAny<string>()), Times.AtMost(1));
                // helper.DeviceMock.Verify(x => 
                //     x.UpdateTokenAsync("token"), Times.AtLeastOnce);
                break;
            case HttpStatusCode.NotFound:
                break;
            case HttpStatusCode.InternalServerError:
            case HttpStatusCode.BadRequest:
            case HttpStatusCode.BadGateway:
                helper.GuiMock.Verify(x =>
                    x.DisplayAlert(It.IsAny<string>()), Times.Once);
                // helper.DeviceMock.Verify(x => 
                //     x.UpdateTokenAsync("token"), Times.Never);
                helper.GuiMock.Verify(x =>
                    x.NavigateToAsync(ApplicationPages.Essential.HomePage, true), Times.Never);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(code), code, null);
        }
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
        using var helper = new HostTestHelper();
        var service = helper.GetService<IAuthenticationService>(HttpStatusCode.OK, TokenResponseDto.Create("token"));

        await service.Login(LoginRequestDto.Create(username, password));

        helper.GuiMock.Verify(x =>
            x.DisplayAlert(It.IsAny<string>()), Times.Once);
        helper.GuiMock.Verify(x =>
            x.NavigateToAsync(ApplicationPages.Essential.HomePage, true), Times.Never);
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
        using var helper = new HostTestHelper();
        var service = helper.GetService<IAuthenticationService>(code, TokenResponseDto.Create("token"));
        if (code == HttpStatusCode.OK)
        {
            helper.StorageMock.Setup(x => x.GetKeyValue(IDeviceSecurity.TokenKeyName))
                .ReturnsAsync("value");
        }

        // using var helper = new TestHelper(nameof(UpdateToken_ExpectedResponsesBehaviorTests));
        // var content = helper.GetJsonContent(ResponseDto.Create());
        // var device = helper.DeviceMock.GetObject(code, content);
        //
        // var service = new UserAuthorizationService(device, new ServerOperationService(device));
        await service.UpdateToken("value");

        switch (code)
        {
            case HttpStatusCode.OK:
                helper.GuiMock.Verify(x =>
                    x.DisplayAlert(It.IsAny<string>()), Times.Never);
                helper.GuiMock.Verify(x =>
                    x.NavigateToAsync(ApplicationPages.Essential.HomePage, It.IsAny<bool>()), Times.AtLeastOnce);

                // helper.DeviceMock.Verify(x => 
                //     x.UpdateTokenAsync("token"), Times.AtLeastOnce);
                break;
            case HttpStatusCode.NotFound:
                helper.GuiMock.Verify(x =>
                    x.DisplayAlert(It.IsAny<string>()), Times.AtLeastOnce);
                helper.GuiMock.Verify(x =>
                    x.NavigateToAsync(ApplicationPages.Essential.SplashPage, It.IsAny<bool>()), Times.Never);
                break;
            case HttpStatusCode.Unauthorized:
                helper.GuiMock.Verify(x =>
                    x.DisplayAlert(It.IsAny<string>()), Times.AtLeastOnce);
                helper.GuiMock.Verify(x =>
                    x.NavigateToAsync(ApplicationPages.Essential.SplashPage, It.IsAny<bool>()), Times.Never);
                helper.GuiMock.Verify(x =>
                    x.NavigateToAsync(ApplicationPages.Essential.LoginPage, It.IsAny<bool>()), Times.AtLeastOnce);

                // helper.DeviceMock.SecurityMock.Verify(x => 
                //     x.DeleteTokenAsync(), Times.AtLeastOnce);
                break;
            case HttpStatusCode.InternalServerError:
            case HttpStatusCode.BadRequest:
            case HttpStatusCode.BadGateway:
                helper.GuiMock.Verify(x =>
                    x.DisplayAlert(It.IsAny<string>()), Times.AtLeastOnce);
                helper.GuiMock.Verify(x =>
                    x.NavigateToAsync(ApplicationPages.Essential.HomePage, true), Times.Never);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(code), code, null);
        }
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("1")]
    [InlineData("12")]
    [InlineData("123")]
    public async void UpdateToken_DataTransferObjectDefensiveValues(string value)
    {
        using var helper = new HostTestHelper();
        var service = helper.GetService<IAuthenticationService>(HttpStatusCode.OK, TokenResponseDto.Create("token"));

        await service.UpdateToken(value);

        helper.GuiMock.Verify(x =>
            x.DisplayAlert(It.IsAny<string>()), Times.AtLeastOnce);

        helper.GuiMock.Verify(x =>
            x.NavigateToAsync(ApplicationPages.Essential.SplashPage, It.IsAny<bool>()), Times.Never);
    }
    #endregion

    #region Logout()
    [Theory]
    [InlineData(HttpStatusCode.OK)]
    [InlineData(HttpStatusCode.BadRequest)]
    public async void Logout_WhenNotFound_OrSuccess(HttpStatusCode httpStatusCode)
    {
        using var helper = new HostTestHelper();
        var service = helper.GetService<IAuthenticationService>(httpStatusCode);

        await service.Logout();
        
        helper.GuiMock.Verify(x => x.NavigateToAsync(ApplicationPages.Essential.LoginPage, It.IsAny<bool>()));
    }
    #endregion
}