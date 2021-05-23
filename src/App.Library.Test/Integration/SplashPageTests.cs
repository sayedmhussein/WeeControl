using System;
using System.Net;
using Moq;
using MySystem.Web.ClientService.Services;
using MySystem.Web.ClientService.Test.Tools;
using MySystem.Web.ClientService.ViewModels;
using MySystem.Web.Shared.Configuration;
using MySystem.Web.Shared.Configuration.Models;
using MySystem.Web.Shared.Dtos;
using MySystem.Web.Shared.Dtos.V1;
using Xunit;
namespace MySystem.Web.ClientService.UnitTest.Integration
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
