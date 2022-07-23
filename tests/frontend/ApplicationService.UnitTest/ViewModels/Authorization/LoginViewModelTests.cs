using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using WeeControl.Frontend.ApplicationService.Essential;
using WeeControl.Frontend.ApplicationService.Essential.ViewModels;
using WeeControl.Frontend.ApplicationService.Services;
using WeeControl.SharedKernel.Essential.DataTransferObjects;
using WeeControl.SharedKernel.RequestsResponses;

namespace WeeControl.Frontend.ApplicationService.UnitTest.ViewModels.Authorization;

public class LoginViewModelTests : ViewModelTestsBase
{
    public LoginViewModelTests() : base(nameof(AuthorizationViewModel))
    {
    }
    
    #region Success And HttpActions
    [Fact]
    public async void SuccessTest()
    {
        var content1 = GetJsonContent(ResponseDto.Create(TokenDtoV1.Create("token", "name", "url")));
        var content2 = GetJsonContent(ResponseDto.Create(TokenDtoV1.Create("token", "name", "url")));
        var expected = new List<Tuple<HttpStatusCode, HttpContent>>()
        {
            new (HttpStatusCode.OK, content1),
            new (HttpStatusCode.OK, content2)
        };
        
        var vm = new AuthorizationViewModel(Mock.GetObject(expected), null)
        {
            UsernameOrEmail = "username",
            Password = "password"
        };
        
        await vm.LoginAsync();
        
        Mock.AlertMock.Verify(x => 
            x.DisplayAlert(It.IsAny<string>()), Times.AtMost(1));
        Mock.SecurityMock.Verify(x => 
            x.UpdateTokenAsync("token"), Times.AtLeastOnce);
        Mock.NavigationMock.Verify(x => 
            x.NavigateToAsync(Pages.Shared.IndexPage,true), Times.Once);
    }
    
    [Theory]
    [InlineData(HttpStatusCode.BadRequest, HttpStatusCode.BadRequest)]
    [InlineData(HttpStatusCode.NotFound, HttpStatusCode.NotFound)]
    public async void WhenBadRequest(HttpStatusCode code1, HttpStatusCode code2)
         {
             var content1 = GetJsonContent(ResponseDto.Create(TokenDtoV1.Create("token", "name", "url")));
             var content2 = GetJsonContent(ResponseDto.Create(TokenDtoV1.Create("token", "name", "url")));
             var expected = new List<Tuple<HttpStatusCode, HttpContent>>()
             {
                 new (code1, content1),
                 new (code2, content2)
             };
        
             var vm = new AuthorizationViewModel(Mock.GetObject(expected), null)
             {
                 UsernameOrEmail = "username",
                 Password = "password"
             };
             
             await vm.LoginAsync();
             
             Mock.AlertMock.Verify(x => 
                 x.DisplayAlert(It.IsAny<string>()), Times.Once);
             Mock.NavigationMock.Verify(x => 
                 x.NavigateToAsync(Pages.Shared.IndexPage,true), Times.Never);
         }
    
    
    #endregion

    #region CommunicationFailure
    [Fact]
    public async void HttpClientIsDefault()
    {
        var client = new HttpClient();
        var vm = new AuthorizationViewModel(Mock.GetObject(client), null)
        {
            UsernameOrEmail = "username",
            Password = "password"
        };
        
        await vm.LoginAsync();
        
        Mock.AlertMock.Verify(x => 
            x.DisplayAlert(It.IsAny<string>()), Times.AtLeast(1));
        Mock.SecurityMock.Verify(x => 
            x.UpdateTokenAsync("token"), Times.Never);
        Mock.NavigationMock.Verify(x => 
            x.NavigateToAsync(Pages.Shared.IndexPage,It.IsAny<bool>()), Times.Never);
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
        var content = GetJsonContent(ResponseDto.Create(TokenDtoV1.Create("token", "name", "url")));
        var vm = new AuthorizationViewModel(Mock.GetObject(HttpStatusCode.OK,content), null)
        {
            UsernameOrEmail = username,
            Password = password
        };
        
        await vm.LoginAsync();
        
        Mock.AlertMock.Verify(x => 
            x.DisplayAlert(It.IsAny<string>()), Times.Once);
        Mock.SecurityMock.Verify(x => 
            x.UpdateTokenAsync(It.IsAny<string>()), Times.Never);
        Mock.NavigationMock.Verify(x => 
            x.NavigateToAsync(Pages.Shared.IndexPage,true), Times.Never);
    }
    #endregion

    #region InvalidCommands
    
    #endregion

    
}