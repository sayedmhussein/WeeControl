using System.Net;
using WeeControl.User.UserApplication.ViewModels.User;

namespace WeeControl.User.UserApplication.Test.ViewModels.User;

public class SetNewPasswordViewModelTests : ViewModelTestsBase
{
    public SetNewPasswordViewModelTests() : base(nameof(SetNewPasswordViewModel))
    {
    }

    [Fact]
    public async void WhenSuccessAndOk()
    {
        var vm = new SetNewPasswordViewModel(mock.GetObject(HttpStatusCode.OK, null!))
        {
            OldPassword = "oldPassword",
            NewPassword = "NewPassword",
            ConfirmNewPassword = "NewPassword"
        };

        await vm.ChangeMyPassword();

        mock.NavigationMock.Verify(x => x.NavigateToAsync(Pages.Home.IndexPage, It.IsAny<bool>()));
    }
    
    [Fact]
    public async void WhenBadRequest()
    {
        var vm = new SetNewPasswordViewModel(mock.GetObject(HttpStatusCode.BadRequest, null!))
 {
     OldPassword = "oldPassword",
     NewPassword = "NewPassword",
     ConfirmNewPassword = "NewPassword"
 };

        await vm.ChangeMyPassword();

        mock.AlertMock.Verify(x => x.DisplayAlert(It.IsAny<string>()));
        mock.NavigationMock.Verify(x => x.NavigateToAsync(Pages.Home.IndexPage, It.IsAny<bool>()), Times.Never);
    }
    
    [Fact]
    public async void WhenUnauthorized()
    {
        var vm = new SetNewPasswordViewModel(mock.GetObject(HttpStatusCode.Unauthorized, null!))
            {
                OldPassword = "oldPassword",
                NewPassword = "NewPassword",
                ConfirmNewPassword = "NewPassword"
            };

        await vm.ChangeMyPassword();

        mock.AlertMock.Verify(x => x.DisplayAlert(It.IsAny<string>()));
        mock.NavigationMock.Verify(x => x.NavigateToAsync(Pages.Home.IndexPage, It.IsAny<bool>()), Times.Never);
    }
    
    [Fact]
    public async void WhenNotFound()
    {
        var vm = new SetNewPasswordViewModel(mock.GetObject(HttpStatusCode.NotFound, null!))
 {
     OldPassword = "oldPassword",
     NewPassword = "NewPassword",
     ConfirmNewPassword = "NewPassword"
 };

        await vm.ChangeMyPassword();

        mock.AlertMock.Verify(x => x.DisplayAlert(It.IsAny<string>()));
        mock.NavigationMock.Verify(x => x.NavigateToAsync(Pages.Home.IndexPage, It.IsAny<bool>()), Times.Never);
    }
    
    [Fact]
    public async void WhenServerCommunicationError()
    {
        var vm = new SetNewPasswordViewModel(mock.GetObject(HttpStatusCode.BadGateway, null!))
 {
     OldPassword = "oldPassword",
     NewPassword = "NewPassword",
     ConfirmNewPassword = "NewPassword"
 };

        await vm.ChangeMyPassword();

        mock.AlertMock.Verify(x => x.DisplayAlert(It.IsAny<string>()));
        mock.NavigationMock.Verify(x => x.NavigateToAsync(Pages.Home.IndexPage, It.IsAny<bool>()), Times.Never);
    }
    
    [Theory]
    [InlineData("", "", "")]
    [InlineData("bla", "bla", "")]
    [InlineData("", "bla", "bla")]
    [InlineData("bla", "", "bla")]
    [InlineData("bla", "bla", "notBla")]
    public async void WhenInvalidProperties(string oldPassword, string newPassword, string confirmPassword)
    {
        var vm = new SetNewPasswordViewModel(mock.GetObject(HttpStatusCode.OK, null!))
        {
            OldPassword = oldPassword,
            NewPassword = newPassword,
            ConfirmNewPassword = confirmPassword
        };

        await vm.ChangeMyPassword();

        mock.AlertMock.Verify(x => x.DisplayAlert(It.IsAny<string>()));
        mock.NavigationMock.Verify(x => x.NavigateToAsync(Pages.Home.IndexPage, It.IsAny<bool>()), Times.Never);
    }
}