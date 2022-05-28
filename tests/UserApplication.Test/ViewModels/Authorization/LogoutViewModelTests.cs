using System.Net;
using WeeControl.SharedKernel.Interfaces;
using WeeControl.SharedKernel.RequestsResponses;
using WeeControl.User.UserApplication.ViewModels.Authentication;

namespace WeeControl.User.UserApplication.Test.ViewModels.Authorization;

public class LogoutViewModelTests
{
    #region Preparation
    private DeviceServiceMock mock;
    
    public LogoutViewModelTests()
    {
        mock = new DeviceServiceMock(nameof(LogoutViewModelTests));
    }
    #endregion

    #region Success

    [Fact]
    public async void WhenNotFound()
    {
        var vm = new LogoutViewModel(mock.GetObject<ResponseDto>(HttpStatusCode.NotFound, null!));

        await vm.LogoutAsync();
        
        mock.SecurityMock.Verify(x => x.DeleteTokenAsync());
        mock.NavigationMock.Verify(x => x.NavigateToAsync(Pages.Authentication.LoginPage, It.IsAny<bool>()));
    }
    
    [Fact]
    public async void WhenBadRequest()
    {
        var vm = new LogoutViewModel(mock.GetObject<ResponseDto>(HttpStatusCode.BadRequest, null!));

        await vm.LogoutAsync();
        
        mock.SecurityMock.Verify(x => x.DeleteTokenAsync());
        mock.NavigationMock.Verify(x => x.NavigateToAsync(Pages.Authentication.LoginPage, It.IsAny<bool>()));
    }
    
    [Fact]
    public async void WhenUnauthorized()
    {
        var vm = new LogoutViewModel(mock.GetObject<ResponseDto>(HttpStatusCode.Unauthorized, null!));

        await vm.LogoutAsync();
        
        mock.SecurityMock.Verify(x => x.DeleteTokenAsync());
        mock.NavigationMock.Verify(x => x.NavigateToAsync(Pages.Authentication.LoginPage, It.IsAny<bool>()));
    }
    #endregion

    #region CommunicationFailure
    [Fact]
    public async void ServerFailure()
    {
        var vm = new LogoutViewModel(mock.GetObject(new HttpClient()));

        await vm.LogoutAsync();
        
        mock.SecurityMock.Verify(x => x.DeleteTokenAsync());
        mock.NavigationMock.Verify(x => x.NavigateToAsync(Pages.Authentication.LoginPage, It.IsAny<bool>()));
    }
    #endregion

    #region InvalidProperties
    #endregion

    #region InvalidCommands
    #endregion

    #region HttpCodes
    #endregion
}