using System.Net;
using Microsoft.AspNetCore.Authorization;
using WeeControl.Core.DataTransferObject.Contexts.User;
using WeeControl.Host.WebApiService;
using WeeControl.Host.WebApiService.Contexts.User;

namespace WeeControl.Host.Test.ApiService.Contexts.User;

public class AuthorizationServiceTests
{
    #region LoginTests
    
    [Fact]
    public async void WhenSuccessUsernameAndPasswordAndOtp()
    {
        using var helper = new TestingServiceHelper();
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
            x.NavigateToAsync(ApplicationPages.HomePage, It.IsAny<bool>()), Times.Once);
    }

    [Theory]
    [InlineData(HttpStatusCode.OK, true)]
    [InlineData(HttpStatusCode.NotFound, false)]
    [InlineData(HttpStatusCode.BadRequest, false)]
    [InlineData(HttpStatusCode.BadGateway, false)]
    [InlineData(HttpStatusCode.InternalServerError, false)]
    public async void Login_ExpectedResponsesBehaviorTests(HttpStatusCode code, bool fnReturn)
    {
        using var helper = new TestingServiceHelper();

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
                    x.NavigateToAsync(ApplicationPages.HomePage, true), Times.Never);
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
        using var helper = new TestingServiceHelper();
        var service = helper.GetService<IAuthenticationService>(HttpStatusCode.OK, TokenResponseDto.Create("token"));

        await service.Login(LoginRequestDto.Create(username, password));

        helper.GuiMock.Verify(x =>
            x.DisplayAlert(It.IsAny<string>()), Times.Once);
        helper.GuiMock.Verify(x =>
            x.NavigateToAsync(ApplicationPages.HomePage, true), Times.Never);
    }
    #endregion
}