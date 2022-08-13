using System.Net;
using System.Net.Http;
using WeeControl.Frontend.ApplicationService.Essential.Legacy;

namespace WeeControl.Frontend.ApplicationService.UnitTest.ViewModels.Authorization;

public class LogoutViewModelTests : ViewModelTestsBase
{
    public LogoutViewModelTests(): base(nameof(LogoutLegacyViewModel))
    {
    }


    [Fact]
    public async void WhenNotFound_OrSuccess()
    {
        var vm = new LogoutLegacyViewModel(Mock.GetObject(HttpStatusCode.NotFound, null!));

        await vm.LogoutAsync();
        
        Mock.SecurityMock.Verify(x => x.DeleteTokenAsync());
        Mock.NavigationMock.Verify(x => x.NavigateToAsync(Pages.Customer.Authentication.LoginPage, It.IsAny<bool>()));
    }
    
    [Fact]
    public async void WhenBadRequest()
    {
        var vm = new LogoutLegacyViewModel(Mock.GetObject(HttpStatusCode.BadRequest, null!));

        await vm.LogoutAsync();
        
        Mock.SecurityMock.Verify(x => x.DeleteTokenAsync());
        Mock.NavigationMock.Verify(x => x.NavigateToAsync(Pages.Customer.Authentication.LoginPage, It.IsAny<bool>()));
    }
    
    [Fact]
    public async void WhenUnauthorized()
    {
        var vm = new LogoutLegacyViewModel(Mock.GetObject(HttpStatusCode.Unauthorized, null!));

        await vm.LogoutAsync();
        
        Mock.SecurityMock.Verify(x => x.DeleteTokenAsync());
        Mock.NavigationMock.Verify(x => x.NavigateToAsync(Pages.Customer.Authentication.LoginPage, It.IsAny<bool>()));
    }
    
    [Fact]
    public async void ServerFailure()
    {
        var vm = new LogoutLegacyViewModel(Mock.GetObject(new HttpClient()));

        await vm.LogoutAsync();
        
        Mock.SecurityMock.Verify(x => x.DeleteTokenAsync());
        Mock.NavigationMock.Verify(x => x.NavigateToAsync(Pages.Customer.Authentication.LoginPage, It.IsAny<bool>()));
    }
}