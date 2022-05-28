using System.Net;
using WeeControl.SharedKernel.DataTransferObjects.Authentication;
using WeeControl.SharedKernel.RequestsResponses;
using WeeControl.User.UserApplication.ViewModels.Authentication;

namespace WeeControl.User.UserApplication.Test.ViewModels.Authorization;

public class LoginViewModelTests : IDisposable
{
    #region Preparation
    private DeviceServiceMock mock;
    
    public LoginViewModelTests()
    {
        mock = new DeviceServiceMock(nameof(LoginViewModelTests));
    }

    public void Dispose()
    {
        //mock = null;
    }
    #endregion
    
    #region Success And HttpActions
    [Fact]
    public async void SuccessTest()
    {
        var vm = new LoginViewModel(mock.GetObject(HttpStatusCode.OK, 
            new ResponseDto<TokenDtoV1>(
                TokenDtoV1.Create("token", "name", "url"))))
        {
            UsernameOrEmail = "username",
            Password = "password"
        };
        
        await vm.LoginAsync();
        
        mock.AlertMock.Verify(x => 
            x.DisplayAlert(It.IsAny<string>()), Times.AtMost(1));
        mock.SecurityMock.Verify(x => 
            x.UpdateTokenAsync("token"), Times.AtLeastOnce);
        mock.NavigationMock.Verify(x => 
            x.NavigateToAsync(Pages.Home.Index,true), Times.Once);
    }
    
    [Theory]
    [InlineData(HttpStatusCode.BadRequest)]
    [InlineData(HttpStatusCode.NotFound)]
    public async void WhenBadRequest(HttpStatusCode code)
         {
             var vm = new LoginViewModel(mock.GetObject(code, 
                 new ResponseDto<TokenDtoV1>(
                     TokenDtoV1.Create("token", "name", "url"))))
             {
                 UsernameOrEmail = "username",
                 Password = "password"
             };
             
             await vm.LoginAsync();
             
             mock.AlertMock.Verify(x => 
                 x.DisplayAlert(It.IsAny<string>()), Times.Once);
             mock.NavigationMock.Verify(x => 
                 x.NavigateToAsync(Pages.Home.Index,true), Times.Never);
         }
    
    
    #endregion

    #region CommunicationFailure
    [Fact]
    public async void HttpClientIsDefault()
    {
        var client = new HttpClient();
        var vm = new LoginViewModel(mock.GetObject(client))
        {
            UsernameOrEmail = "username",
            Password = "password"
        };
        
        await vm.LoginAsync();
        
        mock.AlertMock.Verify(x => 
            x.DisplayAlert(It.IsAny<string>()), Times.AtLeast(1));
        mock.SecurityMock.Verify(x => 
            x.UpdateTokenAsync("token"), Times.Never);
        mock.NavigationMock.Verify(x => 
            x.NavigateToAsync(Pages.Home.Index,It.IsAny<bool>()), Times.Never);
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
        var vm = new LoginViewModel(mock.GetObject(HttpStatusCode.OK,
            new ResponseDto<TokenDtoV1>(TokenDtoV1.Create("token", "name", "url"))))
        {
            UsernameOrEmail = username,
            Password = password
        };
        
        await vm.LoginAsync();
        
        mock.AlertMock.Verify(x => 
            x.DisplayAlert(It.IsAny<string>()), Times.Once);
        mock.SecurityMock.Verify(x => 
            x.UpdateTokenAsync(It.IsAny<string>()), Times.Never);
        mock.NavigationMock.Verify(x => 
            x.NavigateToAsync(Pages.Home.Index,true), Times.Never);
    }
    #endregion

    #region InvalidCommands
    
    #endregion
}