using System;
using System.Collections.Generic;
using System.Net;
using WeeControl.Common.SharedKernel.Contexts.Authentication;
using WeeControl.Frontend.AppService;
using WeeControl.Frontend.AppService.GuiInterfaces.Authorization;

namespace WeeControl.Frontend.Service.UnitTest.Contexts.Essential;

public class UserAuthorizationServiceTests
{
    #region Login()
    [Fact]
    public async void WhenSuccessUsernameAndPasswordAndOtp()
    {
        using var helper = new TestHelper(nameof(WhenSuccessUsernameAndPasswordAndOtp));
        var service = helper.GetService<IAuthorizationService>(
            new List<Tuple<HttpStatusCode, object?>>()
            {
                new (HttpStatusCode.OK, TokenResponseDto.Create("token", "name")),
                new (HttpStatusCode.OK, TokenResponseDto.Create("token", "name")),
                new (HttpStatusCode.OK, TokenResponseDto.Create("token", "name"))
            });
        
        Assert.True(await service.Login("username", "password"));
        Assert.True(await service.UpdateToken("0000"));
        helper.DeviceMock.Verify(x => 
            x.NavigateToAsync(ApplicationPages.SplashPage, It.IsAny<bool>()), Times.Once);
    }
    
    [Theory]
    [InlineData(HttpStatusCode.OK, true)]
    [InlineData(HttpStatusCode.NotFound, false)]
    [InlineData(HttpStatusCode.BadRequest, false)]
    [InlineData(HttpStatusCode.BadGateway, false)]
    [InlineData(HttpStatusCode.InternalServerError, false)]
    public async void Login_ExpectedResponsesBehaviorTests(HttpStatusCode code, bool fnReturn)
    {
        using var helper = new TestHelper(nameof(Login_ExpectedResponsesBehaviorTests));

        var expected = new List<Tuple<HttpStatusCode, object>>()
        {
            new (code, TokenResponseDto.Create("token", "name")),
            new (code, TokenResponseDto.Create("token", "name")),
            new (code, TokenResponseDto.Create("token", "name"))
        };
        
        var service = helper.GetService<IAuthorizationService>(expected);

        Assert.Equal(fnReturn, await service.Login("username", "password"));

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
                helper.DeviceMock.Verify(x => 
                    x.DisplayAlert(It.IsAny<string>()), Times.Once);
                // helper.DeviceMock.Verify(x => 
                //     x.UpdateTokenAsync("token"), Times.Never);
                helper.DeviceMock.Verify(x => 
                    x.NavigateToAsync(ApplicationPages.HomePage,true), Times.Never);
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
        using var helper = new TestHelper(nameof(Login_DataTransferObjectDefensiveValues));
        var service = helper.GetService<IAuthorizationService>(HttpStatusCode.OK, TokenResponseDto.Create("token", "name"));

        await service.Login(username,password);
        
        helper.DeviceMock.Verify(x => 
            x.DisplayAlert(It.IsAny<string>()), Times.Once);
        helper.DeviceMock.Verify(x => 
            x.NavigateToAsync(ApplicationPages.HomePage,true), Times.Never);
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
        using var helper = new TestHelper(nameof(Logout_WhenNotFound_OrSuccess));
        var service = helper.GetService<IAuthorizationService>(code, TokenResponseDto.Create("token", "name"));
        
        // using var helper = new TestHelper(nameof(UpdateToken_ExpectedResponsesBehaviorTests));
        // var content = helper.GetJsonContent(ResponseDto.Create());
        // var device = helper.DeviceMock.GetObject(code, content);
        //
        // var service = new UserAuthorizationService(device, new ServerOperationService(device));
        await service.UpdateToken("value");
    
        switch (code)
        {
            case HttpStatusCode.OK:
                helper.DeviceMock.Verify(x => 
                    x.DisplayAlert(It.IsAny<string>()), Times.Never);
                helper.DeviceMock.Verify(x => 
                    x.NavigateToAsync(ApplicationPages.SplashPage,It.IsAny<bool>()), Times.AtLeastOnce);
                
                // helper.DeviceMock.Verify(x => 
                //     x.UpdateTokenAsync("token"), Times.AtLeastOnce);
                break;
            case HttpStatusCode.NotFound:
                helper.DeviceMock.Verify(x => 
                    x.DisplayAlert(It.IsAny<string>()), Times.AtLeastOnce);
                helper.DeviceMock.Verify(x => 
                    x.NavigateToAsync(ApplicationPages.SplashPage,It.IsAny<bool>()), Times.Never);
                break;
            case HttpStatusCode.Unauthorized:
                helper.DeviceMock.Verify(x => 
                    x.DisplayAlert(It.IsAny<string>()), Times.AtLeastOnce);
                helper.DeviceMock.Verify(x => 
                    x.NavigateToAsync(ApplicationPages.SplashPage,It.IsAny<bool>()), Times.Never);
                helper.DeviceMock.Verify(x => 
                    x.NavigateToAsync(ApplicationPages.Essential.UserPage,It.IsAny<bool>()), Times.AtLeastOnce);
                
                // helper.DeviceMock.SecurityMock.Verify(x => 
                //     x.DeleteTokenAsync(), Times.AtLeastOnce);
                break;
            case HttpStatusCode.InternalServerError:
            case HttpStatusCode.BadRequest:
            case HttpStatusCode.BadGateway:
                helper.DeviceMock.Verify(x => 
                    x.DisplayAlert(It.IsAny<string>()), Times.AtLeastOnce);
                helper.DeviceMock.Verify(x => 
                    x.NavigateToAsync(ApplicationPages.HomePage,true), Times.Never);
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
        using var helper = new TestHelper(nameof(Logout_WhenNotFound_OrSuccess));
        var service = helper.GetService<IAuthorizationService>(HttpStatusCode.OK, TokenResponseDto.Create("token"));
        
        await service.UpdateToken(value);
    
        helper.DeviceMock.Verify(x => 
            x.DisplayAlert(It.IsAny<string>()), Times.AtLeastOnce);
        
        helper.DeviceMock.Verify(x => 
            x.NavigateToAsync(ApplicationPages.SplashPage,It.IsAny<bool>()), Times.Never);
    }
    #endregion
    
    #region Logout()
    [Theory]
    [InlineData(HttpStatusCode.OK, true)]
    [InlineData(HttpStatusCode.BadRequest, false)]
    public async void Logout_WhenNotFound_OrSuccess(HttpStatusCode httpStatusCode, bool fnReturn)
    {
        using var helper = new TestHelper(nameof(Logout_WhenNotFound_OrSuccess));
        var service = helper.GetService<IAuthorizationService>(httpStatusCode, null);

        Assert.Equal(fnReturn, await service.Logout());
        helper.DeviceMock.Verify(x => x.NavigateToAsync(ApplicationPages.SplashPage, It.IsAny<bool>()));
    }
    #endregion
}