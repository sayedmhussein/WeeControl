using System.Net;
using WeeControl.Frontend.ApplicationService.Essential.ViewModels;

namespace WeeControl.Frontend.ApplicationService.UnitTest.ViewModels.User;

public class ForgotMyPasswordViewModelTests : ViewModelTestsBase
{
    public ForgotMyPasswordViewModelTests() : base(nameof(PasswordResetLegacyViewModel))
    {
    }

    [Fact]
    public async void WhenSuccessAndOk()
    {
        var vm = new PasswordResetLegacyViewModel(Mock.GetObject(HttpStatusCode.OK, null!))
        {
            Email = "email@email.com",
            Username = "username"
        };

        await vm.RequestPasswordReset();

        Mock.NavigationMock.Verify(x => x.NavigateToAsync(Pages.Essential.Authentication.LoginPage, It.IsAny<bool>()));
    }
    
    [Fact]
    public async void WhenBadRequest()
    {
        var vm = new PasswordResetLegacyViewModel(Mock.GetObject(HttpStatusCode.BadRequest, null!))
        {
            Email = "email@email.com",
            Username = "username"
        };

        await vm.RequestPasswordReset();
        
        Mock.NavigationMock.Verify(x => x.NavigateToAsync(Pages.Essential.Authentication.LoginPage, It.IsAny<bool>()), Times.Never);
    }
    
    [Fact]
    public async void ServerCommunication()
    {
        var vm = new PasswordResetLegacyViewModel(Mock.GetObject(HttpStatusCode.BadGateway, null!))
        {
            Email = "email@email.com",
            Username = "username"
        };

        await vm.RequestPasswordReset();
        
        Mock.NavigationMock.Verify(x => x.NavigateToAsync(Pages.Essential.Authentication.LoginPage, It.IsAny<bool>()), Times.Never);
    }
    
    [Theory]
    [InlineData("", "")]
    [InlineData("   ", "       ")]
    [InlineData("email@email.com", "")]
    [InlineData("", "username")]
    public async void WhenInvalidProperties(string email, string username)
    {
        var vm = new PasswordResetLegacyViewModel(Mock.GetObject(HttpStatusCode.OK, null!))
        {
            Email = email,
            Username = username
        };

        await vm.RequestPasswordReset();
        
        Mock.NavigationMock.Verify(x => x.NavigateToAsync(Pages.Essential.Authentication.LoginPage, It.IsAny<bool>()), Times.Never);
        Assert.False(vm.IsLoading);
    }
}