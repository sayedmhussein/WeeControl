using System;
using System.Net.Http;
using Moq;
using Sayed.MySystem.ClientService.Services;
using Sayed.MySystem.ClientService.ViewModels;
using Xunit;

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

            Action action = () => device.Verify(x => x.NavigateToPageAsync("SplashPage"), Times.Once);

            Assert.Throws<Moq.MockException>(action);
        }

        [Fact]
        public async void WhenConnectingToServerAndExcptionOccures_()
        {
            var device = new Mock<IDevice>();
            device.Setup(x => x.Token).Returns(new Random().NextDouble().ToString());
            device.Setup(x => x.Internet).Returns(true);
            //
            var service = new Mock<IClientServices>();
            service.Setup(x => x.HttpClient.PostAsJsonAsync(It.IsAny<string>(), It.IsAny<It.IsAnyType>())).Throws<Exception>();

            var vm = new SplashViewModel(device.Object, service.Object);
            await vm.RefreshTokenCommand.ExecuteAsync(null);

            device.Verify(x => x.DisplayMessageAsync("Exception", It.IsAny<string>()), Times.Once);
        }
    }
}
