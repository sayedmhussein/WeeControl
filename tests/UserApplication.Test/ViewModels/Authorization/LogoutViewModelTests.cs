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
        var vm = new LogoutViewModel(Mock.GetObject(HttpStatusCode.NotFound, null!));

        await vm.LogoutAsync();
        
        Mock.SecurityMock.Verify(x => x.DeleteTokenAsync());
        Mock.NavigationMock.Verify(x => x.NavigateToAsync(Pages.Essential.Authentication.LoginPage, It.IsAny<bool>()));
    }
    
    [Fact]
    public async void WhenBadRequest()
    {
        var vm = new LogoutViewModel(Mock.GetObject(HttpStatusCode.BadRequest, null!));

        await vm.LogoutAsync();
        
        Mock.SecurityMock.Verify(x => x.DeleteTokenAsync());
        Mock.NavigationMock.Verify(x => x.NavigateToAsync(Pages.Essential.Authentication.LoginPage, It.IsAny<bool>()));
    }
    
    [Fact]
    public async void WhenUnauthorized()
    {
        var vm = new LogoutViewModel(Mock.GetObject(HttpStatusCode.Unauthorized, null!));

        await vm.LogoutAsync();
        
        Mock.SecurityMock.Verify(x => x.DeleteTokenAsync());
        Mock.NavigationMock.Verify(x => x.NavigateToAsync(Pages.Essential.Authentication.LoginPage, It.IsAny<bool>()));
    }
    
    [Fact]
    public async void ServerFailure()
    {
        var vm = new LogoutViewModel(Mock.GetObject(new HttpClient()));

        await vm.LogoutAsync();
        
        Mock.SecurityMock.Verify(x => x.DeleteTokenAsync());
        Mock.NavigationMock.Verify(x => x.NavigateToAsync(Pages.Essential.Authentication.LoginPage, It.IsAny<bool>()));
    }
}