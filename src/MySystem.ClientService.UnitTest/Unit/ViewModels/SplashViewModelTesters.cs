using System;
using System.Net.Http;
using Moq;
using Xunit;
using Sayed.MySystem.ClientService.Services;
using Sayed.MySystem.ClientService.ViewModels;
using Moq.Protected;
using System.Threading.Tasks;
using System.Net;
using System.Threading;
using Sayed.MySystem.Shared.Configuration.Models;
using Microsoft.Extensions.Logging;
using System.Text;
using Sayed.MySystem.Shared.Dtos.V1;
using Sayed.MySystem.Shared.Dtos;
using Sayed.MySystem.ClientService.Test.Tools;

namespace Sayed.MySystem.ClientService.Test.Unit.ViewModels
{
    public class SplashViewModelTesters
    {
        #region Constructor
        [Fact]
        public void Constructor_WhenNullService_ThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new SplashViewModel(null));
        }
        #endregion

        #region Commands
        [Fact]
        public async void WhenNoInternet_DisplayMessageToUserThenTerminateApp()
        {
            var device = TestMocks.GetDeviceMock();
            device.Setup(x => x.Internet).Returns(false);
            //
            var service = TestMocks.GetClientServicesMock(device.Object);

            await new SplashViewModel(service.Object).RefreshTokenCommand.ExecuteAsync(null);

            device.Verify(x => x.DisplayMessageAsync(IDevice.Message.NoInternet), Times.AtLeastOnce);
            device.Verify(x => x.TerminateAppAsync(), Times.AtLeastOnce);
        }

        [Fact]
        public async void WhenTokenIsEmpty_OpenLoginPage()
        {
            var device = TestMocks.GetDeviceMock();
            device.Setup(x => x.Token).Returns(string.Empty) ;
            //
            var service = TestMocks.GetClientServicesMock(device.Object);

            await new SplashViewModel(service.Object).RefreshTokenCommand.ExecuteAsync(null);

            device.Verify(x => x.NavigateToPageAsync("LoginPage"), Times.Once);
        }

        [Fact]
        public async void WhenServerResponseSuccess_OpenHomePageAndTokenSameAsSentFromServer()
        {
            var stringContent = new StringContent(new RequestDto<string>("Device", "Token").SerializeToJson(), Encoding.UTF8, "application/json");
            //
            var deviceMock = TestMocks.GetDeviceMock();
            var apiMock = TestMocks.GetApiMock();
            var loggerMock = TestMocks.GetLoggerMock();
            var handlerMock = TestMocks.GetHttpMessageHandlerMock(HttpStatusCode.OK, stringContent);

            var service = new ClientServices(deviceMock.Object, apiMock.Object, loggerMock.Object, handlerMock.Object)
            {
                SystemUnderTest = true
            };
            service.HttpClientInstance.Timeout = TimeSpan.FromSeconds(1);

            await new SplashViewModel(service).RefreshTokenCommand.ExecuteAsync(null);

            deviceMock.Verify(x => x.NavigateToPageAsync("HomePage"), Times.Once);
            deviceMock.Verify(x => x.Token, Times.AtLeastOnce);
        }

        [Fact]
        public async void WhenServerResponseNotSuccess_OpenLoginPage()
        {
            var deviceMock = new Mock<IDevice>();
            deviceMock.Setup(x => x.Token).Returns(new Random().NextDouble().ToString());
            deviceMock.Setup(x => x.Internet).Returns(true);
            //
            var apiMock = new Mock<IApi>();
            apiMock.Setup(p => p.Base).Returns(new Uri("http://bla.com"));
            //
            var handlerMock = TestMocks.GetHttpMessageHandlerMock(new HttpResponseMessage(HttpStatusCode.InternalServerError));

            var service = new ClientServices(deviceMock.Object, apiMock.Object, new Mock<ILogger>().Object, handlerMock.Object)
            {
                SystemUnderTest = true
            };
            service.HttpClientInstance.Timeout = TimeSpan.FromSeconds(1);

            var vm = new SplashViewModel(service);

            await vm.RefreshTokenCommand.ExecuteAsync(null);

            deviceMock.Verify(x => x.NavigateToPageAsync("LoginPage"), Times.Once);
        }

        [Fact]
        public async void WhenConnectingToServerAndWebExcptionOccures_AppShouldBeTerminated()
        {
            var deviceMock = new Mock<IDevice>();
            deviceMock.Setup(x => x.Token).Returns(new Random().NextDouble().ToString());
            deviceMock.Setup(x => x.Internet).Returns(true);
            //
            var handlerMock = TestMocks.GetHttpMessageHandlerMock(() => throw new WebException());

            var service = new ClientServices(deviceMock.Object, new Mock<IApi>().Object, new Mock<ILogger>().Object, handlerMock.Object)
            {
                SystemUnderTest = true
            };
            service.HttpClientInstance.Timeout = TimeSpan.FromSeconds(1);

            var vm = new SplashViewModel(service);
            
            await vm.RefreshTokenCommand.ExecuteAsync(null);

            deviceMock.Verify(x => x.TerminateAppAsync(), Times.Once);
        }

        [Fact]
        public async void WhenConnectingToServerAndOtherExcptionOccures_AppShouldBeTerminated()
        {
            var device = new Mock<IDevice>();
            device.Setup(x => x.Token).Returns(new Random().NextDouble().ToString());
            device.Setup(x => x.Internet).Returns(true);
            //
            var api = new Mock<IApi>();
            //
            //
            var handler = TestMocks.GetHttpMessageHandlerMock(() => throw new Exception());

            var service = new ClientServices(device.Object, api.Object, new Mock<ILogger>().Object, handler.Object);
            service.SystemUnderTest = true;
            service.HttpClientInstance.Timeout = TimeSpan.FromSeconds(1);

            var vm = new SplashViewModel(service);

            await vm.RefreshTokenCommand.ExecuteAsync(null);

            device.Verify(x => x.TerminateAppAsync(), Times.Once);
        }
        #endregion
    }
}
