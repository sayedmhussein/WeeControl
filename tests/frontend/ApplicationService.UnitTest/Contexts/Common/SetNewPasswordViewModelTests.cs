using System.Net;
using WeeControl.Frontend.ApplicationService.Interfaces;
using WeeControl.Frontend.ApplicationService.Services;

namespace WeeControl.Frontend.ApplicationService.UnitTest.Contexts.Common;

public class SetNewPasswordViewModelTests : ViewModelTestsBase
{
    public SetNewPasswordViewModelTests() : base(nameof(PasswordChangeViewModel))
    {
    }

    [Fact]
    public async void WhenSuccessAndOk()
    {
        var device = Mock.GetObject(HttpStatusCode.OK, null!);
        var vm = GetViewModel(device);

        await vm.ChangeMyPassword();

        Mock.NavigationMock.Verify(x => x.NavigateToAsync(Pages.Shared.IndexPage, It.IsAny<bool>()));
    }
    
    [Fact]
    public async void WhenBadRequest()
    {
        var vm = GetViewModel(Mock.GetObject(HttpStatusCode.BadRequest, null!));

        await vm.ChangeMyPassword();

        Mock.AlertMock.Verify(x => x.DisplayAlert(It.IsAny<string>()));
        Mock.NavigationMock.Verify(x => x.NavigateToAsync(Pages.Shared.IndexPage, It.IsAny<bool>()), Times.Never);
    }
    
    [Fact]
    public async void WhenUnauthorized()
    {
        var vm = GetViewModel(Mock.GetObject(HttpStatusCode.Unauthorized, null!));

        await vm.ChangeMyPassword();

        Mock.AlertMock.Verify(x => x.DisplayAlert(It.IsAny<string>()));
        Mock.NavigationMock.Verify(x => x.NavigateToAsync(Pages.Shared.IndexPage, It.IsAny<bool>()), Times.Never);
    }
    
    [Fact]
    public async void WhenNotFound()
    {
        var vm = GetViewModel(Mock.GetObject(HttpStatusCode.NotFound, null!));

        await vm.ChangeMyPassword();

        Mock.AlertMock.Verify(x => x.DisplayAlert(It.IsAny<string>()));
        Mock.NavigationMock.Verify(x => x.NavigateToAsync(Pages.Shared.IndexPage, It.IsAny<bool>()), Times.Never);
    }
    
    [Fact]
    public async void WhenServerCommunicationError()
    {
        var vm = GetViewModel(Mock.GetObject(HttpStatusCode.BadGateway, null!));

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
        var vm = GetViewModel(Mock.GetObject(HttpStatusCode.OK, null!));
        vm.Model.OldPassword = oldPassword;
        vm.Model.NewPassword = newPassword;
        vm.Model.ConfirmPassword = confirmPassword;

        await vm.ChangeMyPassword();

        Mock.AlertMock.Verify(x => x.DisplayAlert(It.IsAny<string>()));
        Mock.NavigationMock.Verify(x => x.NavigateToAsync(Pages.Shared.IndexPage, It.IsAny<bool>()), Times.Never);
    }

    private PasswordChangeViewModel GetViewModel(IDevice device)
    {
        return new PasswordChangeViewModel(device, new ServerOperationService(device))
        {
            Model = 
            {
                OldPassword = "oldPassword",
                NewPassword = "NewPassword",
                ConfirmPassword = "NewPassword"
            }
        };
    }
}