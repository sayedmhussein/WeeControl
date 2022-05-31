using System.Net;
using WeeControl.SharedKernel.RequestsResponses;
using WeeControl.User.UserApplication.ViewModels.Admin;

namespace WeeControl.User.UserApplication.Test.ViewModels.Admin;

public class AdminViewModelTests
{
    private DeviceServiceMock mock;

    public AdminViewModelTests()
    {
        mock = new DeviceServiceMock(nameof(AdminViewModelTests));
    }

    [Fact]
    public async void WhenSuccess()
    {
        var vm = new AdminViewModel(mock.GetObject<ResponseDto>(HttpStatusCode.OK, null!));

        await vm.GetListOfUsers();
    }

    [Theory]
    [InlineData(HttpStatusCode.Unauthorized)]
    [InlineData(HttpStatusCode.Forbidden)]
    public async void WhenUnauthorizedOrForbidden(HttpStatusCode code)
    {
        var vm = new AdminViewModel(mock.GetObject<ResponseDto>(code, null!));

        await vm.GetListOfUsers();
        
        Assert.Empty(vm.ListOfUsers);
        mock.AlertMock.Verify(x => 
            x.DisplayAlert(It.IsAny<string>()), Times.AtLeast(1));
    }
}