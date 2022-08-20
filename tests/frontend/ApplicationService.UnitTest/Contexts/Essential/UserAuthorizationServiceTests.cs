using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using WeeControl.Frontend.ApplicationService.Contexts.Essential.Models;
using WeeControl.Frontend.ApplicationService.Contexts.Essential.Services;
using WeeControl.Frontend.ApplicationService.Interfaces;
using WeeControl.SharedKernel.Contexts.Essential.DataTransferObjects.User;
using WeeControl.SharedKernel.RequestsResponses;

namespace WeeControl.Frontend.ApplicationService.UnitTest.Contexts.Essential;

public class UserAuthorizationServiceTests
{
    #region IsAuthorized()
    [Theory]
    [InlineData(true, true, true)]
    [InlineData(false, true, false)]
    [InlineData(true, false, false)]
    [InlineData(false, false, false)]
    public async void IsAuthorized_WhiteBoxTests(bool tokenValid, bool authenticated, bool result)
    {
        using var helper = new TestHelper(nameof(IsAuthorized_WhiteBoxTests));
        var server = new Mock<IServerOperation>();
        server.Setup(x => x.IsTokenValid()).ReturnsAsync(tokenValid);
        helper.DeviceMock.SecurityMock.Setup(x => x.IsAuthenticatedAsync()).ReturnsAsync(authenticated);
        
        var service = new UserAuthorizationService(helper.DeviceMock.GetObject(new HttpClient()), server.Object);
        
        Assert.Equal(result, await service.IsAuthorized());
    }
    #endregion

    #region Login()
    [Theory]
    [InlineData(HttpStatusCode.OK)]
    [InlineData(HttpStatusCode.NotFound)]
    [InlineData(HttpStatusCode.BadRequest)]
    [InlineData(HttpStatusCode.BadGateway)]
    [InlineData(HttpStatusCode.InternalServerError)]
    public async void Login_ExpectedResponsesBehaviorTests(HttpStatusCode code)
    {
        using var helper = new TestHelper(nameof(Login_ExpectedResponsesBehaviorTests));
        var content1 = helper.GetJsonContent(ResponseDto.Create(AuthenticationResponseDto.Create("token", "name")));
        var content2 = helper.GetJsonContent(ResponseDto.Create(AuthenticationResponseDto.Create("token", "name")));
        var expected = new List<Tuple<HttpStatusCode, HttpContent>>()
        {
            new (code, content1),
            new (code, content2)
        };
        
        var device = helper.DeviceMock.GetObject(expected);
        var service = new UserAuthorizationService(device, helper.GetServer(device));
        
        await service.Login(new LoginModel() { UsernameOrEmail = "username", Password = "password"});

        switch (code)
        {
            case HttpStatusCode.OK:
                helper.DeviceMock.AlertMock.Verify(x => 
                    x.DisplayAlert(It.IsAny<string>()), Times.AtMost(1));
                helper.DeviceMock.SecurityMock.Verify(x => 
                    x.UpdateTokenAsync("token"), Times.AtLeastOnce);
                helper.DeviceMock.NavigationMock.Verify(x => 
                    x.NavigateToAsync(Pages.Essential.SplashPage,true), Times.Once);
                break;
            case HttpStatusCode.NotFound:
                break;
            case HttpStatusCode.InternalServerError:
            case HttpStatusCode.BadRequest:
            case HttpStatusCode.BadGateway:
                helper.DeviceMock.AlertMock.Verify(x => 
                    x.DisplayAlert(It.IsAny<string>()), Times.Once);
                helper.DeviceMock.SecurityMock.Verify(x => 
                    x.UpdateTokenAsync("token"), Times.Never);
                helper.DeviceMock.NavigationMock.Verify(x => 
                    x.NavigateToAsync(Pages.Essential.HomePage,true), Times.Never);
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
        var content = helper.GetJsonContent(ResponseDto.Create(AuthenticationResponseDto.Create("token", "name")));

        var device = helper.DeviceMock.GetObject(HttpStatusCode.OK, content);
        var vm = new UserAuthorizationService(device, helper.GetServer(device));

        await vm.Login(new LoginModel() { UsernameOrEmail = username, Password = password});
        
        helper.DeviceMock.AlertMock.Verify(x => 
            x.DisplayAlert(It.IsAny<string>()), Times.Once);
        helper.DeviceMock.SecurityMock.Verify(x => 
            x.UpdateTokenAsync(It.IsAny<string>()), Times.Never);
        helper.DeviceMock.NavigationMock.Verify(x => 
            x.NavigateToAsync(Pages.Essential.HomePage,true), Times.Never);
    }
    #endregion

    #region UpdateToken()
    
    #endregion

    #region Logout()
    [Fact]
    public async void Logout_WhenNotFound_OrSuccess()
    {
        using var helper = new TestHelper(nameof(Logout_WhenNotFound_OrSuccess));
        var device = helper.DeviceMock.GetObject(HttpStatusCode.NotFound, null!);
        var vm = new UserAuthorizationService(device, helper.GetServer(device));

        await vm.Logout();
        
        helper.DeviceMock.SecurityMock.Verify(x => x.DeleteTokenAsync());
        helper.DeviceMock.NavigationMock.Verify(x => x.NavigateToAsync(Pages.Essential.SplashPage, It.IsAny<bool>()));
    }
    
    [Fact]
    public async void Logout_WhenBadRequest()
    {
        using var helper = new TestHelper(nameof(Logout_WhenBadRequest));
        var device = helper.DeviceMock.GetObject(HttpStatusCode.BadRequest, null!);
        var vm = new UserAuthorizationService(device, helper.GetServer(device));

        await vm.Logout();
        
        helper.DeviceMock.SecurityMock.Verify(x => x.DeleteTokenAsync());
        helper.DeviceMock.NavigationMock.Verify(x => x.NavigateToAsync(Pages.Essential.SplashPage, It.IsAny<bool>()));
    }
    
    [Fact]
    public async void Logout_WhenUnauthorized()
    {
        using var helper = new TestHelper(nameof(Logout_WhenUnauthorized));
        var device = helper.DeviceMock.GetObject(HttpStatusCode.Unauthorized, null!);
        var vm = new UserAuthorizationService(device, helper.GetServer(device));

        await vm.Logout();
        
        helper.DeviceMock.SecurityMock.Verify(x => x.DeleteTokenAsync());
        //helper.DeviceMock.NavigationMock.Verify(x => x.NavigateToAsync(Pages.Essential.UserPage, It.IsAny<bool>()));
    }
    
    [Fact]
    public async void Logout_ServerFailure()
    {
        using var helper = new TestHelper(nameof(Logout_ServerFailure));
        var device = helper.DeviceMock.GetObject(new HttpClient());
        var vm = new UserAuthorizationService(device, helper.GetServer(device));

        await vm.Logout();
        
        helper.DeviceMock.SecurityMock.Verify(x => x.DeleteTokenAsync());
        //helper.DeviceMock.NavigationMock.Verify(x => x.NavigateToAsync(Pages.Essential.UserPage, It.IsAny<bool>()));
    }
    #endregion
}