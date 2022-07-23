using System.Net;
using WeeControl.Frontend.ApplicationService.Essential.ViewModels;
using WeeControl.Frontend.ApplicationService.Interfaces;
using WeeControl.Frontend.ApplicationService.Services;

namespace WeeControl.Frontend.ApplicationService.UnitTest.ViewModels.User;

public class ForgotMyPasswordViewModelTests : ViewModelTestsBase
{
    public ForgotMyPasswordViewModelTests() : base(nameof(PasswordResetViewModel))
    {
    }

    [Fact]
    public async void WhenSuccessAndOk()
    {
        var vm = 
            GetViewModel(Mock.GetObject(HttpStatusCode.OK, null!), "email@email.com", "username");

        await vm.RequestPasswordReset();

        Mock.NavigationMock.Verify(x => x.NavigateToAsync(Pages.Essential.Authentication.LoginPage, It.IsAny<bool>()));
    }
    
    [Fact]
    public async void WhenBadRequest()
    {
        var vm = GetViewModel(Mock.GetObject(HttpStatusCode.BadRequest, null!), "email@email.com", "username");

        await vm.RequestPasswordReset();
        
        Mock.NavigationMock.Verify(x => x.NavigateToAsync(Pages.Essential.Authentication.LoginPage, It.IsAny<bool>()), Times.Never);
    }
    
    [Fact]
    public async void ServerCommunication()
    {
        var vm =  GetViewModel(Mock.GetObject(HttpStatusCode.BadGateway, null!), "email@email.com",
            "username");

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
        var vm = GetViewModel(Mock.GetObject(HttpStatusCode.OK, null!), email, username);

        await vm.RequestPasswordReset();
        
        Mock.NavigationMock.Verify(x => x.NavigateToAsync(Pages.Essential.Authentication.LoginPage, It.IsAny<bool>()), Times.Never);
        Assert.False(vm.IsLoading);
    }

    private PasswordResetViewModel GetViewModel(IDevice device, string email, string username)
    {
        return new PasswordResetViewModel(device, new ServerOperationService(device))
        {
            Email = email,
            Username = username
        };
    }
}