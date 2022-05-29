using System.Net;
using WeeControl.SharedKernel.RequestsResponses;
using WeeControl.User.UserApplication.ViewModels.User;

namespace WeeControl.User.UserApplication.Test.ViewModels.User;

public class RegisterViewModelTests
{
    private DeviceServiceMock mock;

    public RegisterViewModelTests()
    {
        mock = new DeviceServiceMock(nameof(RegisterViewModelTests));
    }
    
    [Fact]
    public async void WhenSuccess()
    {
        var vm = new RegisterViewModel(mock.GetObject<ResponseDto>(HttpStatusCode.OK, null!));
        vm.Email = "email@email.com";
        vm.Username = "username";
        vm.Password = "password";

        await vm.RegisterAsync();

        mock.NavigationMock.Verify(x => x.NavigateToAsync(Pages.Home.Index, It.IsAny<bool>()));
    }
    
    [Fact]
    public async void WhenBadRequest()
    {
        var vm = new RegisterViewModel(mock.GetObject<ResponseDto>(HttpStatusCode.BadRequest, null!))
        {
            Email = "email@email.com",
            Username = "username",
            Password = "password"
        };

        await vm.RegisterAsync();
        
        mock.AlertMock.Verify(x => x.DisplayAlert(It.IsAny<string>()));
        mock.NavigationMock.Verify(x => x.NavigateToAsync(Pages.Home.Index, It.IsAny<bool>()), Times.Never);
        Assert.False(vm.IsLoading);
    }
    
    [Fact]
    public async void WhenConflict()
    {
        var vm = new RegisterViewModel(mock.GetObject<ResponseDto>(HttpStatusCode.Conflict, null!))
        {
            Email = "email@email.com",
            Username = "username",
            Password = "password"
        };

        await vm.RegisterAsync();
        
        mock.AlertMock.Verify(x => x.DisplayAlert(It.IsAny<string>()));
        mock.NavigationMock.Verify(x => x.NavigateToAsync(Pages.Home.Index, It.IsAny<bool>()), Times.Never);
        Assert.False(vm.IsLoading);
    }
    
    [Fact]
    public async void WhenServerCommunicationError()
    {
        var vm = new RegisterViewModel(mock.GetObject<ResponseDto>(HttpStatusCode.BadGateway, null!))
        {
            Email = "email@email.com",
            Username = "username",
            Password = "password"
        };

        await vm.RegisterAsync();
        
        mock.AlertMock.Verify(x => x.DisplayAlert(It.IsAny<string>()));
        mock.NavigationMock.Verify(x => x.NavigateToAsync(Pages.Home.Index, It.IsAny<bool>()), Times.Never);
        Assert.False(vm.IsLoading);
    }
    
    [Theory]
    [InlineData("", "", "")]
    [InlineData("email@email.com", "", "")]
    [InlineData("", "username", "")]
    [InlineData("", "", "password")]
    public async void WhenInvalidProperties(string email, string username, string password)
    {
        var vm = new RegisterViewModel(mock.GetObject<ResponseDto>(HttpStatusCode.OK, null!))
        {
            Email = email,
            Username = username,
            Password = password
        };

        await vm.RegisterAsync();
        
        mock.AlertMock.Verify(x => x.DisplayAlert(It.IsAny<string>()));
        mock.NavigationMock.Verify(x => x.NavigateToAsync(Pages.Home.Index, It.IsAny<bool>()), Times.Never);
        Assert.False(vm.IsLoading);
    }
}