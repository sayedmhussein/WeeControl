using System.Net;
using WeeControl.SharedKernel.Essential.DataTransferObjects;
using WeeControl.SharedKernel.RequestsResponses;
using WeeControl.User.UserApplication.ViewModels.Authentication;

namespace WeeControl.User.UserApplication.Test.ViewModels.Authorization;

public class LoginViewModelTests : ViewModelTestsBase
{
    public LoginViewModelTests() : base(nameof(LoginViewModel))
    {
    }
    
    #region Success And HttpActions
    [Fact]
    public async void SuccessTest()
    {
        var content1 = GetJsonContent(new ResponseDto<TokenDtoV1>(TokenDtoV1.Create("token", "name", "url")));
        var content2 = GetJsonContent(new ResponseDto<TokenDtoV1>(TokenDtoV1.Create("token", "name", "url")));
        var expected = new List<Tuple<HttpStatusCode, HttpContent>>()
        {
            new (HttpStatusCode.OK, content1),
            new (HttpStatusCode.OK, content2)
        };
        
        var vm = new LoginViewModel(Mock.GetObject(expected))
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
            x.NavigateToAsync(Pages.Home.IndexPage,true), Times.Once);
    }
    
    [Theory]
    [InlineData(HttpStatusCode.BadRequest, HttpStatusCode.BadRequest)]
    [InlineData(HttpStatusCode.NotFound, HttpStatusCode.NotFound)]
    public async void WhenBadRequest(HttpStatusCode code1, HttpStatusCode code2)
         {
             var content1 = GetJsonContent(new ResponseDto<TokenDtoV1>(TokenDtoV1.Create("token", "name", "url")));
             var content2 = GetJsonContent(new ResponseDto<TokenDtoV1>(TokenDtoV1.Create("token", "name", "url")));
             var expected = new List<Tuple<HttpStatusCode, HttpContent>>()
             {
                 new (code1, content1),
                 new (code2, content2)
             };
        
             var vm = new LoginViewModel(Mock.GetObject(expected))
             {
                 UsernameOrEmail = "username",
                 Password = "password"
             };
             
             await vm.LoginAsync();
             
             Mock.AlertMock.Verify(x => 
                 x.DisplayAlert(It.IsAny<string>()), Times.Once);
             Mock.NavigationMock.Verify(x => 
                 x.NavigateToAsync(Pages.Home.IndexPage,true), Times.Never);
         }
    
    
    #endregion

    #region CommunicationFailure
    [Fact]
    public async void HttpClientIsDefault()
    {
        var client = new HttpClient();
        var vm = new LoginViewModel(Mock.GetObject(client))
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
            x.NavigateToAsync(Pages.Home.IndexPage,It.IsAny<bool>()), Times.Never);
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
        var content = GetJsonContent(new ResponseDto<TokenDtoV1>(TokenDtoV1.Create("token", "name", "url")));
        var vm = new LoginViewModel(Mock.GetObject(HttpStatusCode.OK,content))
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
            x.NavigateToAsync(Pages.Home.IndexPage,true), Times.Never);
    }
    #endregion

    #region InvalidCommands
    
    #endregion

    
}