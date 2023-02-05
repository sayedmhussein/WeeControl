using System.Net;
using WeeControl.Core.DataTransferObject.User;
using WeeControl.Frontend.AppService;
using WeeControl.Frontend.AppService.Internals.Interfaces;
using WeeControl.Frontend.AppService.Internals.Services;

namespace WeeControl.Frontend.Service.UnitTest.Services;

public class ServerOperationTests
{
    private Mock<IDeviceData> deviceDataMock;
    private Mock<IDeviceSecurity> deviceSecurityMock;

    public ServerOperationTests()
    {
        deviceDataMock = new Mock<IDeviceData>();
        deviceDataMock.SetupAllProperties();

        deviceSecurityMock = new Mock<IDeviceSecurity>();
        deviceSecurityMock.SetupAllProperties();
        deviceSecurityMock.Setup(x => x.IsAuthenticatedAsync()).ReturnsAsync(true);
    }

    [Fact]
    public async void WhenNoTokenIsSaved_ReturnFalse()
    {
        deviceSecurityMock.Setup(x => x.IsAuthenticatedAsync()).ReturnsAsync(false);

        var service = new ServerOperationService(deviceDataMock.Object, deviceSecurityMock.Object);

        Assert.False(await service.RefreshToken());
    }

    [Fact]
    public async void RefreshTokenTest()
    {
        var helper = new TestHelper(nameof(RefreshTokenTest));
        helper.DeviceMock.Setup(x => x.GetKeyValue(It.IsAny<string>())).ReturnsAsync("token");

        var service = helper.GetService<IServerOperation>(HttpStatusCode.OK, TokenResponseDto.Create("token", "fullname"));

        Assert.True(await service.RefreshToken());
    }
}