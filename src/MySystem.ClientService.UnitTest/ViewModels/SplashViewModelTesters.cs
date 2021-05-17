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

namespace Sayed.MySystem.ClientService.UnitTest.ViewModels
{
    public class SplashViewModelTesters
    {
        [Fact]
        public async void WhenNoInternet_DisplayMessageToUser()
        {
            var device = new Mock<IDevice>();
            device.Setup(x => x.Token).Returns(new Random().NextDouble().ToString());
            device.Setup(x => x.Internet).Returns(false);
            //
            var service = new Mock<IClientServices>();

            var vm = new SplashViewModel(device.Object, service.Object);
            await vm.RefreshTokenCommand.ExecuteAsync(null);

            device.Verify(x => x.DisplayMessageAsync(IDevice.Message.NoInternet), Times.AtLeastOnce);
        }

        [Fact]
        public async void WhenTokenIsEmpty_OpenLoginPage()
        {
            var device = new Mock<IDevice>();
            device.Setup(x => x.Token).Returns(string.Empty) ;
            device.Setup(x => x.Internet).Returns(true);
            //
            var service = new Mock<IClientServices>();

            var vm = new SplashViewModel(device.Object, service.Object);
            await vm.RefreshTokenCommand.ExecuteAsync(null);

            device.Verify(x => x.NavigateToPageAsync("LoginPage"), Times.Once);
        }

        [Fact]
        public async void WhenTokenIsEmpty_OpenLoginPageExceptionIfTestMistake()
        {
            var device = new Mock<IDevice>();
            device.Setup(x => x.Token).Returns(string.Empty);
            device.Setup(x => x.Internet).Returns(true);
            //
            var service = new Mock<IClientServices>();

            var vm = new SplashViewModel(device.Object, service.Object);
            await vm.RefreshTokenCommand.ExecuteAsync(null);

            void action() => device.Verify(x => x.NavigateToPageAsync("SplashPage"), Times.Once);

            Assert.Throws<MockException>(action);
        }

        [Fact]
        public async void WhenConnectingToServerAndExcptionOccures_AppShouldBeTerminatedAsResultOfException()
        {
            var device = new Mock<IDevice>();
            device.Setup(x => x.Token).Returns(new Random().NextDouble().ToString());
            //
            var api = new Mock<IApi>();
            //
            device.Setup(x => x.Internet).Returns(true);
            //
            var handler = new Mock<HttpMessageHandler>();
            //handler.Protected()
            //    .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            //    .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.OK });

            handler.Protected()
                 .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                 .ReturnsAsync(() => throw new Exception()) ;

            var service = new ClientServices(device.Object, api.Object, new Mock<ILogger>().Object, handler.Object);
            service.HttpClientInstance.Timeout = TimeSpan.FromSeconds(1);

            var vm = new SplashViewModel(device.Object, service);
            
            await vm.RefreshTokenCommand.ExecuteAsync(null);

            device.Verify(x => x.TerminateApp(), Times.Once);
        }
    }
}
