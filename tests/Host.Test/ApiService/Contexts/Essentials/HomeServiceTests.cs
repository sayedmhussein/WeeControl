using System.Net;
using WeeControl.Core.DataTransferObject.Contexts.Essentials;
using WeeControl.Core.SharedKernel.Contexts.Essentials;
using WeeControl.Host.WebApiService.Contexts.Essentials;
using WeeControl.Host.WebApiService.DeviceInterfaces;

namespace WeeControl.Host.Test.ApiService.Contexts.Essentials;

public class HomeServiceTests
{
    [Fact]
    public async void WhenSuccess_ReturnAllItems()
    {
        using var helper = new HostTestHelper(HttpStatusCode.OK, new HomeResponseDto
        {
            FullName = "Full Name", Notifications = new[] {new HomeNotificationModel()}
        });
        var service = helper.GetService<IHomeService>();

        var success = await service.PullData();

        Assert.True(success);
    }

    [Theory]
    [InlineData(HttpStatusCode.NotFound)]
    [InlineData(HttpStatusCode.BadRequest)]
    public async void WhenNotSuccess(HttpStatusCode httpStatusCode)
    {
        using var helper = new HostTestHelper(httpStatusCode);
        var service = helper.GetService<IHomeService>();

        var success = await service.PullData();

        Assert.False(success);
        helper.GuiMock.Verify(x => x.DisplayAlert(It.IsAny<string>(), It.IsAny<IGui.Severity>()), Times.Once);
    }
}