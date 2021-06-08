using System;
using System.Net;
using Moq;
using MySystem.Persistence.ClientService.Services;
using MySystem.Persistence.ClientService.Test.Tools;
using MySystem.Persistence.ClientService.ViewModels;
using MySystem.Persistence.Shared.Configuration;
using MySystem.Persistence.Shared.Configuration.Models;
using MySystem.Persistence.Shared.Dtos;
using MySystem.Persistence.Shared.Dtos.V1;
using Xunit;
namespace MySystem.Persistence.ClientService.UnitTest.Integration
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
