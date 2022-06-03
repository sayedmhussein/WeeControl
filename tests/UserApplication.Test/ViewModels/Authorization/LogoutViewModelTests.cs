using System.Net;
using WeeControl.User.UserApplication.ViewModels.Authentication;

namespace WeeControl.User.UserApplication.Test.ViewModels.Authorization;

public class LogoutViewModelTests : ViewModelTestsBase
{
    public LogoutViewModelTests(): base(nameof(LogoutViewModel))
    {
    }


    [Fact]
    public async void WhenNotFound_OrSuccess()
    {
        var vm = new LogoutViewModel(mock.GetObject(HttpStatusCode.NotFound, null!));

        await vm.LogoutAsync();
        
        mock.SecurityMock.Verify(x => x.DeleteTokenAsync());
        mock.NavigationMock.Verify(x => x.NavigateToAsync(Pages.Authentication.LoginPage, It.IsAny<bool>()));
    }
    
    [Fact]
    public async void WhenBadRequest()
    {
        var vm = new LogoutViewModel(mock.GetObject(HttpStatusCode.BadRequest, null!));

        await vm.LogoutAsync();
        
        mock.SecurityMock.Verify(x => x.DeleteTokenAsync());
        mock.NavigationMock.Verify(x => x.NavigateToAsync(Pages.Authentication.LoginPage, It.IsAny<bool>()));
    }
    
    [Fact]
    public async void WhenUnauthorized()
    {
        var vm = new LogoutViewModel(mock.GetObject(HttpStatusCode.Unauthorized, null!));

        await vm.LogoutAsync();
        
        mock.SecurityMock.Verify(x => x.DeleteTokenAsync());
        mock.NavigationMock.Verify(x => x.NavigateToAsync(Pages.Authentication.LoginPage, It.IsAny<bool>()));
    }
    
    [Fact]
    public async void ServerFailure()
    {
        var vm = new LogoutViewModel(mock.GetObject(new HttpClient()));

        await vm.LogoutAsync();
        
        mock.SecurityMock.Verify(x => x.DeleteTokenAsync());
        mock.NavigationMock.Verify(x => x.NavigateToAsync(Pages.Authentication.LoginPage, It.IsAny<bool>()));
    }
}