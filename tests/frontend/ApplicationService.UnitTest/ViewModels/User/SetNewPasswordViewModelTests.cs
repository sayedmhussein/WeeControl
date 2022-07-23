using System.Net;
using WeeControl.Frontend.ApplicationService.Essential.ViewModels;

namespace WeeControl.Frontend.ApplicationService.UnitTest.ViewModels.User;

public class SetNewPasswordViewModelTests : ViewModelTestsBase
{
    public SetNewPasswordViewModelTests() : base(nameof(PasswordChangeLegacyViewModel))
    {
    }

    [Fact]
    public async void WhenSuccessAndOk()
    {
        var vm = new PasswordChangeLegacyViewModel(Mock.GetObject(HttpStatusCode.OK, null!))
        {
            OldPassword = "oldPassword",
            NewPassword = "NewPassword",
            ConfirmNewPassword = "NewPassword"
        };

        await vm.ChangeMyPassword();

        Mock.NavigationMock.Verify(x => x.NavigateToAsync(Pages.Shared.IndexPage, It.IsAny<bool>()));
    }
    
    [Fact]
    public async void WhenBadRequest()
    {
        var vm = new PasswordChangeLegacyViewModel(Mock.GetObject(HttpStatusCode.BadRequest, null!))
 {
     OldPassword = "oldPassword",
     NewPassword = "NewPassword",
     ConfirmNewPassword = "NewPassword"
 };

        await vm.ChangeMyPassword();

        Mock.AlertMock.Verify(x => x.DisplayAlert(It.IsAny<string>()));
        Mock.NavigationMock.Verify(x => x.NavigateToAsync(Pages.Shared.IndexPage, It.IsAny<bool>()), Times.Never);
    }
    
    [Fact]
    public async void WhenUnauthorized()
    {
        var vm = new PasswordChangeLegacyViewModel(Mock.GetObject(HttpStatusCode.Unauthorized, null!))
            {
                OldPassword = "oldPassword",
                NewPassword = "NewPassword",
                ConfirmNewPassword = "NewPassword"
            };

        await vm.ChangeMyPassword();

        Mock.AlertMock.Verify(x => x.DisplayAlert(It.IsAny<string>()));
        Mock.NavigationMock.Verify(x => x.NavigateToAsync(Pages.Shared.IndexPage, It.IsAny<bool>()), Times.Never);
    }
    
    [Fact]
    public async void WhenNotFound()
    {
        var vm = new PasswordChangeLegacyViewModel(Mock.GetObject(HttpStatusCode.NotFound, null!))
 {
     OldPassword = "oldPassword",
     NewPassword = "NewPassword",
     ConfirmNewPassword = "NewPassword"
 };

        await vm.ChangeMyPassword();

        Mock.AlertMock.Verify(x => x.DisplayAlert(It.IsAny<string>()));
        Mock.NavigationMock.Verify(x => x.NavigateToAsync(Pages.Shared.IndexPage, It.IsAny<bool>()), Times.Never);
    }
    
    [Fact]
    public async void WhenServerCommunicationError()
    {
        var vm = new PasswordChangeLegacyViewModel(Mock.GetObject(HttpStatusCode.BadGateway, null!))
 {
     OldPassword = "oldPassword",
     NewPassword = "NewPassword",
     ConfirmNewPassword = "NewPassword"
 };

        await vm.ChangeMyPassword();

        Mock.AlertMock.Verify(x => x.DisplayAlert(It.IsAny<string>()));
        Mock.NavigationMock.Verify(x => x.NavigateToAsync(Pages.Shared.IndexPage, It.IsAny<bool>()), Times.Never);
    }
    
    [Theory]
    [InlineData("", "", "")]
    [InlineData("bla", "bla", "")]
    [InlineData("", "bla", "bla")]
    [InlineData("bla", "", "bla")]
    [InlineData("bla", "bla", "notBla")]
    public async void WhenInvalidProperties(string oldPassword, string newPassword, string confirmPassword)
    {
        var vm = new PasswordChangeLegacyViewModel(Mock.GetObject(HttpStatusCode.OK, null!))
        {
            OldPassword = oldPassword,
            NewPassword = newPassword,
            ConfirmNewPassword = confirmPassword
        };

        await vm.ChangeMyPassword();

        Mock.AlertMock.Verify(x => x.DisplayAlert(It.IsAny<string>()));
        Mock.NavigationMock.Verify(x => x.NavigateToAsync(Pages.Shared.IndexPage, It.IsAny<bool>()), Times.Never);
    }
}