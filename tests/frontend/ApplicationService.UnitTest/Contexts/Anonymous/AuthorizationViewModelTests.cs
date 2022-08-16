using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using WeeControl.Frontend.ApplicationService.Contexts.Anonymous.ViewModels;
using WeeControl.Frontend.ApplicationService.Interfaces;
using WeeControl.Frontend.ApplicationService.Services;
using WeeControl.SharedKernel.Contexts.Essential.DataTransferObjects;
using WeeControl.SharedKernel.RequestsResponses;

namespace WeeControl.Frontend.ApplicationService.UnitTest.Contexts.Anonymous;

public class AuthorizationViewModelTests : ViewModelTestsBase
{
    public AuthorizationViewModelTests() : base(nameof(AuthorizationViewModel))
    {
    }

    #region Login
    #region Success And HttpActions
    [Fact]
    public async void LoginSuccessTest()
    {
        var content1 = GetJsonContent(ResponseDto.Create(AuthenticationResponseDto.Create("token", "name")));
        var content2 = GetJsonContent(ResponseDto.Create(AuthenticationResponseDto.Create("token", "name")));
        var expected = new List<Tuple<HttpStatusCode, HttpContent>>()
        {
            new (HttpStatusCode.OK, content1),
            new (HttpStatusCode.OK, content2)
        };

        var vm = GetViewModel(Mock.GetObject(expected), "username", "password");

        await vm.Login();
        
        Mock.AlertMock.Verify(x => 
            x.DisplayAlert(It.IsAny<string>()), Times.AtMost(1));
        Mock.SecurityMock.Verify(x => 
            x.UpdateTokenAsync("token"), Times.AtLeastOnce);
        Mock.NavigationMock.Verify(x => 
            x.NavigateToAsync(Pages.Essential.HomePage,true), Times.Once);
    }
    
    [Theory]
    [InlineData(HttpStatusCode.BadRequest, HttpStatusCode.BadRequest)]
    [InlineData(HttpStatusCode.NotFound, HttpStatusCode.NotFound)]
    public async void LoginWhenBadRequest(HttpStatusCode code1, HttpStatusCode code2)
    {
        var content1 = GetJsonContent(ResponseDto.Create(AuthenticationResponseDto.Create("token", "name")));
        var content2 = GetJsonContent(ResponseDto.Create(AuthenticationResponseDto.Create("token", "name")));
        var expected = new List<Tuple<HttpStatusCode, HttpContent>>()
        {
            new (code1, content1),
            new (code2, content2)
        };
        
        var vm = GetViewModel(Mock.GetObject(expected), "username", "password");
             
        await vm.Login();
             
        Mock.AlertMock.Verify(x => 
            x.DisplayAlert(It.IsAny<string>()), Times.Once);
        Mock.NavigationMock.Verify(x => 
            x.NavigateToAsync(Pages.Essential.HomePage,true), Times.Never);
    }
    
    
    #endregion

    #region CommunicationFailure
    [Fact]
    public async void HttpClientIsDefault()
    {
        var client = new HttpClient();
        var vm = GetViewModel(Mock.GetObject(client), "username", "password");

        await vm.Login();
        
        Mock.AlertMock.Verify(x => 
            x.DisplayAlert(It.IsAny<string>()), Times.AtLeast(1));
        Mock.SecurityMock.Verify(x => 
            x.UpdateTokenAsync("token"), Times.Never);
        Mock.NavigationMock.Verify(x => 
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
        var content = GetJsonContent(ResponseDto.Create(AuthenticationResponseDto.Create("token", "name")));
        var vm = GetViewModel(Mock.GetObject(HttpStatusCode.OK, content), username, password);

        await vm.Login();
        
        Mock.AlertMock.Verify(x => 
            x.DisplayAlert(It.IsAny<string>()), Times.Once);
        Mock.SecurityMock.Verify(x => 
            x.UpdateTokenAsync(It.IsAny<string>()), Times.Never);
        Mock.NavigationMock.Verify(x => 
            x.NavigateToAsync(Pages.Essential.HomePage,true), Times.Never);
    }
    #endregion
    

    #endregion

    #region Logout
    [Fact]
    public async void WhenNotFound_OrSuccess()
    {
        var vm = new AuthorizationViewModel(Mock.GetObject(HttpStatusCode.NotFound, null!));

        await vm.Logout();
        
        Mock.SecurityMock.Verify(x => x.DeleteTokenAsync());
        Mock.NavigationMock.Verify(x => x.NavigateToAsync(Pages.Customer.Authentication.LoginPage, It.IsAny<bool>()));
    }
    
    [Fact]
    public async void WhenBadRequest()
    {
        var vm = new AuthorizationViewModel(Mock.GetObject(HttpStatusCode.BadRequest, null!));

        await vm.Logout();
        
        Mock.SecurityMock.Verify(x => x.DeleteTokenAsync());
        Mock.NavigationMock.Verify(x => x.NavigateToAsync(Pages.Customer.Authentication.LoginPage, It.IsAny<bool>()));
    }
    
    [Fact]
    public async void WhenUnauthorized()
    {
        var vm = new AuthorizationViewModel(Mock.GetObject(HttpStatusCode.Unauthorized, null!));

        await vm.Logout();
        
        Mock.SecurityMock.Verify(x => x.DeleteTokenAsync());
        Mock.NavigationMock.Verify(x => x.NavigateToAsync(Pages.Customer.Authentication.LoginPage, It.IsAny<bool>()));
    }
    
    [Fact]
    public async void ServerFailure()
    {
        var vm = new AuthorizationViewModel(Mock.GetObject(new HttpClient()));

        await vm.Logout();
        
        Mock.SecurityMock.Verify(x => x.DeleteTokenAsync());
        Mock.NavigationMock.Verify(x => x.NavigateToAsync(Pages.Customer.Authentication.LoginPage, It.IsAny<bool>()));
    }
    #endregion

    private AuthorizationViewModel GetViewModel(IDevice device, string usernameOrEmail, string password)
    {
        return new AuthorizationViewModel(device, new ServerOperationService(device))
        {
            LoginModel =
            {
                UsernameOrEmail = usernameOrEmail,
                Password = password
            }
        };
    }
}