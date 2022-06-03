using System.Net;
using WeeControl.User.UserApplication.ViewModels.User;

namespace WeeControl.User.UserApplication.Test.ViewModels.User;

public class ForgotMyPasswordViewModelTests : ViewModelTestsBase
{
    public ForgotMyPasswordViewModelTests() : base(nameof(ForgotMyPasswordViewModel))
    {
    }

    [Fact]
    public async void WhenSuccessAndOk()
    {
        var vm = new ForgotMyPasswordViewModel(mock.GetObject(HttpStatusCode.OK, null!))
        {
            Email = "email@email.com",
            Username = "username"
        };

        await vm.RequestPasswordReset();

        mock.NavigationMock.Verify(x => x.NavigateToAsync(Pages.Authentication.LoginPage, It.IsAny<bool>()));
    }
    
    [Fact]
    public async void WhenBadRequest()
    {
        var vm = new ForgotMyPasswordViewModel(mock.GetObject(HttpStatusCode.BadRequest, null!))
        {
            Email = "email@email.com",
            Username = "username"
        };

        await vm.RequestPasswordReset();
        
        mock.NavigationMock.Verify(x => x.NavigateToAsync(Pages.Authentication.LoginPage, It.IsAny<bool>()), Times.Never);
    }
    
    [Fact]
    public async void ServerCommunication()
    {
        var vm = new ForgotMyPasswordViewModel(mock.GetObject(HttpStatusCode.BadGateway, null!))
        {
            Email = "email@email.com",
            Username = "username"
        };

        await vm.RequestPasswordReset();
        
        mock.NavigationMock.Verify(x => x.NavigateToAsync(Pages.Authentication.LoginPage, It.IsAny<bool>()), Times.Never);
    }
    
    [Theory]
    [InlineData("", "")]
    [InlineData("   ", "       ")]
    [InlineData("email@email.com", "")]
    [InlineData("", "username")]
    public async void WhenInvalidProperties(string email, string username)
    {
        var vm = new ForgotMyPasswordViewModel(mock.GetObject(HttpStatusCode.OK, null!))
        {
            Email = email,
            Username = username
        };

        await vm.RequestPasswordReset();
        
        mock.NavigationMock.Verify(x => x.NavigateToAsync(Pages.Authentication.LoginPage, It.IsAny<bool>()), Times.Never);
        Assert.False(vm.IsLoading);
    }
}