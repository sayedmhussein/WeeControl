using System.Net;
using WeeControl.SharedKernel.RequestsResponses;
using WeeControl.User.UserApplication.ViewModels.User;

namespace WeeControl.User.UserApplication.Test.ViewModels.User;

public class SetNewPasswordViewModelTests
{
    private DeviceServiceMock mock;

    public SetNewPasswordViewModelTests()
    {
        mock = new DeviceServiceMock(nameof(ForgotMyPasswordViewModelTests));
    }

    [Fact]
    public async void WhenSuccessAndOk()
    {
        var vm = new SetNewPasswordViewModel(mock.GetObject<ResponseDto>(HttpStatusCode.OK, null!));
        vm.OldPassword = "oldPassword";
        vm.NewPassword = "NewPassword";
        vm.ConfirmNewPassword = "NewPassword";
        
        await vm.ChangeMyPassword();

        mock.NavigationMock.Verify(x => x.NavigateToAsync(Pages.Home.Index, It.IsAny<bool>()));
    }
    
    [Fact]
    public async void WhenBadRequest()
    {
        var vm = new SetNewPasswordViewModel(mock.GetObject<ResponseDto>(HttpStatusCode.BadRequest, null!));
        vm.OldPassword = "oldPassword";
        vm.NewPassword = "NewPassword";
        vm.ConfirmNewPassword = "NewPassword";
        
        await vm.ChangeMyPassword();

        mock.AlertMock.Verify(x => x.DisplayAlert(It.IsAny<string>()));
        mock.NavigationMock.Verify(x => x.NavigateToAsync(Pages.Home.Index, It.IsAny<bool>()), Times.Never);
    }
    
    [Fact]
    public async void WhenUnauthorized()
    {
        var vm = new SetNewPasswordViewModel(mock.GetObject<ResponseDto>(HttpStatusCode.Unauthorized, null!));
        vm.OldPassword = "oldPassword";
        vm.NewPassword = "NewPassword";
        vm.ConfirmNewPassword = "NewPassword";
        
        await vm.ChangeMyPassword();

        mock.AlertMock.Verify(x => x.DisplayAlert(It.IsAny<string>()));
        mock.NavigationMock.Verify(x => x.NavigateToAsync(Pages.Home.Index, It.IsAny<bool>()), Times.Never);
    }
    
    [Fact]
    public async void WhenNotFound()
    {
        var vm = new SetNewPasswordViewModel(mock.GetObject<ResponseDto>(HttpStatusCode.NotFound, null!));
        vm.OldPassword = "oldPassword";
        vm.NewPassword = "NewPassword";
        vm.ConfirmNewPassword = "NewPassword";
        
        await vm.ChangeMyPassword();

        mock.AlertMock.Verify(x => x.DisplayAlert(It.IsAny<string>()));
        mock.NavigationMock.Verify(x => x.NavigateToAsync(Pages.Home.Index, It.IsAny<bool>()), Times.Never);
    }
    
    [Fact]
    public async void WhenServerCommunicatioError()
    {
        var vm = new SetNewPasswordViewModel(mock.GetObject<ResponseDto>(HttpStatusCode.BadGateway, null!));
        vm.OldPassword = "oldPassword";
        vm.NewPassword = "NewPassword";
        vm.ConfirmNewPassword = "NewPassword";
        
        await vm.ChangeMyPassword();

        mock.AlertMock.Verify(x => x.DisplayAlert(It.IsAny<string>()));
        mock.NavigationMock.Verify(x => x.NavigateToAsync(Pages.Home.Index, It.IsAny<bool>()), Times.Never);
    }
    
    [Theory]
    [InlineData("", "", "")]
    [InlineData("bla", "bla", "")]
    [InlineData("", "bla", "bla")]
    [InlineData("bla", "", "bla")]
    [InlineData("bla", "bla", "notBla")]
    public async void WhenInvalidProperties(string oldPassword, string newPassword, string confirmPassword)
    {
        var vm = new SetNewPasswordViewModel(mock.GetObject<ResponseDto>(HttpStatusCode.OK, null!));
        vm.OldPassword = oldPassword;
        vm.NewPassword = newPassword;
        vm.ConfirmNewPassword = confirmPassword;
        
        await vm.ChangeMyPassword();

        mock.AlertMock.Verify(x => x.DisplayAlert(It.IsAny<string>()));
        mock.NavigationMock.Verify(x => x.NavigateToAsync(Pages.Home.Index, It.IsAny<bool>()), Times.Never);
    }
}