using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using WeeControl.Frontend.ApplicationService.Contexts.Essential.Models;
using WeeControl.Frontend.ApplicationService.Contexts.Essential.Services;
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
    public async void IsAuthorized_Tests(bool tokenValid, bool authenticated, bool result)
    {
        using var helper = new TestHelper(nameof(IsAuthorized_Tests));
        helper.ServerOperationMock.Setup(x => x.IsTokenValid()).ReturnsAsync(tokenValid);
        helper.DeviceMock.SecurityMock.Setup(x => x.IsAuthenticatedAsync()).ReturnsAsync(authenticated);
        
        var service = new UserAuthorizationService(helper.DeviceMock.GetObject(new HttpClient()), helper.ServerOperationMock.Object);
        
        Assert.Equal(result, await service.IsAuthorized());
    }
    #endregion

    #region Login()
    #region Success And HttpActions
    [Fact]
    public async void LoginSuccessTest()
    {
        using var helper = new TestHelper(nameof(LoginSuccessTest));
        var content1 = helper.GetJsonContent(ResponseDto.Create(AuthenticationResponseDto.Create("token", "name")));
        var content2 = helper.GetJsonContent(ResponseDto.Create(AuthenticationResponseDto.Create("token", "name")));
        var expected = new List<Tuple<HttpStatusCode, HttpContent>>()
        {
            new (HttpStatusCode.OK, content1),
            new (HttpStatusCode.OK, content2)
        };

        var device = helper.DeviceMock.GetObject(expected);
        var vm = new UserAuthorizationService(device, helper.GetServer(device));

        await vm.Login(new LoginModel() { UsernameOrEmail = "username", Password = "password"});
        
        helper.DeviceMock.AlertMock.Verify(x => 
            x.DisplayAlert(It.IsAny<string>()), Times.AtMost(1));
        helper.DeviceMock.SecurityMock.Verify(x => 
            x.UpdateTokenAsync("token"), Times.AtLeastOnce);
        helper.DeviceMock.NavigationMock.Verify(x => 
            x.NavigateToAsync(Pages.Essential.HomePage,true), Times.Once);
    }
    
    [Theory]
    [InlineData(HttpStatusCode.BadRequest, HttpStatusCode.BadRequest)]
    [InlineData(HttpStatusCode.NotFound, HttpStatusCode.NotFound)]
    public async void LoginWhenBadRequest(HttpStatusCode code1, HttpStatusCode code2)
    {
        using var helper = new TestHelper(nameof(LoginWhenBadRequest));
        var content1 = helper.GetJsonContent(ResponseDto.Create(AuthenticationResponseDto.Create("token", "name")));
        var content2 = helper.GetJsonContent(ResponseDto.Create(AuthenticationResponseDto.Create("token", "name")));
        var expected = new List<Tuple<HttpStatusCode, HttpContent>>()
        {
            new (code1, content1),
            new (code2, content2)
        };

        var device = helper.DeviceMock.GetObject(expected);
        var vm = new UserAuthorizationService(device, helper.GetServer(device));
             
        await vm.Login(new LoginModel() { UsernameOrEmail = "username", Password = "password"});
             
        helper.DeviceMock.AlertMock.Verify(x => 
            x.DisplayAlert(It.IsAny<string>()), Times.Once);
        helper.DeviceMock.NavigationMock.Verify(x => 
            x.NavigateToAsync(Pages.Essential.HomePage,true), Times.Never);
    }
    
    
    #endregion

    #region CommunicationFailure
    [Fact]
    public async void HttpClientIsDefault()
    {
        using var helper = new TestHelper(nameof(LoginSuccessTest));
        var device = helper.DeviceMock.GetObject(new HttpClient());
        var vm = new UserAuthorizationService(device, helper.GetServer(device));

        await vm.Login(new LoginModel() { UsernameOrEmail = "username", Password = "password"});
        
        helper.DeviceMock.AlertMock.Verify(x => 
            x.DisplayAlert(It.IsAny<string>()), Times.AtLeast(1));
        helper.DeviceMock.SecurityMock.Verify(x => 
            x.UpdateTokenAsync("token"), Times.Never);
        helper.DeviceMock.NavigationMock.Verify(x => 
            x.NavigateToAsync(Pages.Essential.HomePage,It.IsAny<bool>()), Times.Never);
    }
    #endregion

    #region InvalidProperties
    [Theory]
    [InlineData("", "")]
    [InlineData("", "password")]
    [InlineData("username", "")]
    [InlineData("    ", "password")]
    [InlineData("username", "    ")]
    [InlineData("   ", "    ")]
    public async void WhenEmptyProperties_DisplayAlertOnly(string username, string password)
    {
        using var helper = new TestHelper(nameof(WhenEmptyProperties_DisplayAlertOnly));
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
    #endregion

    #region Logout
    [Fact]
    public async void WhenNotFound_OrSuccess()
    {
        using var helper = new TestHelper(nameof(WhenEmptyProperties_DisplayAlertOnly));
        var vm = new UserAuthorizationService(helper.DeviceMock.GetObject(HttpStatusCode.NotFound, null!), helper.ServerOperationMock.Object);

        await vm.Logout();
        
        helper.DeviceMock.SecurityMock.Verify(x => x.DeleteTokenAsync());
        helper.DeviceMock.NavigationMock.Verify(x => x.NavigateToAsync(Pages.Essential.UserPage, It.IsAny<bool>()));
    }
    
    [Fact]
    public async void WhenBadRequest()
    {
        using var helper = new TestHelper(nameof(WhenEmptyProperties_DisplayAlertOnly));
        var vm = new UserAuthorizationService(helper.DeviceMock.GetObject(HttpStatusCode.BadRequest, null!), helper.ServerOperationMock.Object);

        await vm.Logout();
        
        helper.DeviceMock.SecurityMock.Verify(x => x.DeleteTokenAsync());
        helper.DeviceMock.NavigationMock.Verify(x => x.NavigateToAsync(Pages.Essential.UserPage, It.IsAny<bool>()));
    }
    
    [Fact]
    public async void WhenUnauthorized()
    {
        using var helper = new TestHelper(nameof(WhenEmptyProperties_DisplayAlertOnly));
        var device = helper.DeviceMock.GetObject(HttpStatusCode.Unauthorized, null!);
        var vm = new UserAuthorizationService(device, helper.GetServer(device));

        await vm.Logout();
        
        helper.DeviceMock.SecurityMock.Verify(x => x.DeleteTokenAsync());
        //helper.DeviceMock.NavigationMock.Verify(x => x.NavigateToAsync(Pages.Essential.UserPage, It.IsAny<bool>()));
    }
    
    [Fact]
    public async void ServerFailure()
    {
        using var helper = new TestHelper(nameof(WhenEmptyProperties_DisplayAlertOnly));
        var device = helper.DeviceMock.GetObject(new HttpClient());
        var vm = new UserAuthorizationService(device, helper.GetServer(device));

        await vm.Logout();
        
        helper.DeviceMock.SecurityMock.Verify(x => x.DeleteTokenAsync());
        //helper.DeviceMock.NavigationMock.Verify(x => x.NavigateToAsync(Pages.Essential.UserPage, It.IsAny<bool>()));
    }
    #endregion
}