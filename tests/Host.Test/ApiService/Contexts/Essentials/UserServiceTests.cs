using System.Net;
using WeeControl.Core.DataTransferObject.Contexts.Essentials;
using WeeControl.Core.SharedKernel.Contexts.Essentials;
using WeeControl.Host.WebApiService.Contexts.Essentials;

namespace WeeControl.Host.Test.ApiService.Contexts.Essentials;

public class UserServiceTests
{
    [Fact]
    public async void WhenSuccess_ReturnAllItems()
    {
        using var helper = new HostTestHelper();
        var responseDto = new HomeResponseDto()
        {
            FullName = "Full Name", Notifications = new []{ new HomeNotificationModel()}
        };
        var service = helper.GetService<IHomeService>(HttpStatusCode.OK, responseDto);

        var success = await service.Refresh();
        
        Assert.True(success);
    }

    [Theory]
    [InlineData(HttpStatusCode.NotFound)]
    [InlineData(HttpStatusCode.BadRequest)]
    public async void WhenNotSuccess(HttpStatusCode httpStatusCode)
    {
        using var helper = new HostTestHelper();
        var service = helper.GetService<IHomeService>(httpStatusCode);

        var success = await service.Refresh();
        
        Assert.False(success);
        helper.GuiMock.Verify(x => x.DisplayAlert(It.IsAny<string>()), Times.Once);
    }
}