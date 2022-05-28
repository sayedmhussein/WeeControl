using System.Net;
using WeeControl.SharedKernel.RequestsResponses;
using WeeControl.User.UserApplication.ViewModels.User;

namespace WeeControl.User.UserApplication.Test.ViewModels.User;

public class ForgotMyPasswordViewModelTests
{
    private DeviceServiceMock mock;

    public ForgotMyPasswordViewModelTests()
    {
        mock = new DeviceServiceMock(nameof(ForgotMyPasswordViewModelTests));
    }

    [Fact]
    public async void WhenSuccessAndOK()
    {
        var vm = new ForgotMyPasswordViewModel(mock.GetObject<ResponseDto>(HttpStatusCode.OK, null!));
        vm.Email = "email@email.com";
        vm.Username = "username";
        
        await vm.RequestPasswordReset();

        mock.NavigationMock.Verify(x => x.NavigateToAsync(Pages.Authentication.LoginPage, It.IsAny<bool>()));
    }
    
    [Fact]
    public async void WhenBadRequest()
    {
        var vm = new ForgotMyPasswordViewModel(mock.GetObject<ResponseDto>(HttpStatusCode.BadRequest, null!));
        vm.Email = "email@email.com";
        vm.Username = "username";
        
        await vm.RequestPasswordReset();
        
        mock.NavigationMock.Verify(x => x.NavigateToAsync(Pages.Authentication.LoginPage, It.IsAny<bool>()), Times.Never);
    }
    
    [Fact]
    public async void ServerCommunication()
    {
        var vm = new ForgotMyPasswordViewModel(mock.GetObject<ResponseDto>(HttpStatusCode.BadGateway, null!));
        vm.Email = "email@email.com";
        vm.Username = "username";
        
        await vm.RequestPasswordReset();
        
        mock.NavigationMock.Verify(x => x.NavigateToAsync(Pages.Authentication.LoginPage, It.IsAny<bool>()), Times.Never);
    }
    
    [Theory]
    [InlineData("", "")]
    public async void WhenInvalidProperties(string email, string username)
    {
        var vm = new ForgotMyPasswordViewModel(mock.GetObject<ResponseDto>(HttpStatusCode.BadGateway, null!));
        vm.Email = email;
        vm.Username = username;
        
        await vm.RequestPasswordReset();
        
        mock.NavigationMock.Verify(x => x.NavigateToAsync(Pages.Authentication.LoginPage, It.IsAny<bool>()), Times.Never);
        Assert.False(vm.IsLoading);
    }
}