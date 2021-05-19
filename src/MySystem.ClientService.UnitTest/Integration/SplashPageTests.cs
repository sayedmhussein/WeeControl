using System;
using System.Net;
using Moq;
using Sayed.MySystem.ClientService.Services;
using Sayed.MySystem.ClientService.Test.Tools;
using Sayed.MySystem.ClientService.ViewModels;
using Sayed.MySystem.Shared.Configuration;
using Sayed.MySystem.Shared.Configuration.Models;
using Sayed.MySystem.Shared.Dtos;
using Sayed.MySystem.Shared.Dtos.V1;
using Xunit;
namespace Sayed.MySystem.ClientService.UnitTest.Integration
{
    public class SplashPageTests
    {
        [Fact]
        public async void Command_WhenTokenExist_OpenHomePage()
        {
            var deviceMock = TestMocks.DeviceMock;
            var handlerMock = TestMocks.GetHttpMessageHandlerMock(HttpStatusCode.OK, new RequestDto<string>("Device", "Token").SerializeToJson());

            var service = new ClientServices(deviceMock.Object, AppSettings.GetAppSetting().Api, null, handlerMock.Object, systemUnderTest: true);

            await new SplashViewModel(service).RefreshTokenCommand.ExecuteAsync(null);

            deviceMock.Verify(x => x.NavigateToPageAsync("HomePage"), Times.Once);
        }
    }
}
