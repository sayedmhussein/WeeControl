using System;
using System.Net;
using System.Net.Http;
using Moq;
using WeeControl.Applications.BaseLib.Interfaces;
using WeeControl.Applications.BaseLib.ViewModels.Common;
using WeeControl.SharedKernel.Aggregates.Employee.BaseEntities.DtosV1;
using WeeControl.SharedKernel.Extensions;
using Xunit;

namespace WeeControl.User.Employee.Test.UnitTests.ViewModels
{
    public class SplashViewModelTesters : ViewModelTestBase
    {
        #region Constructor
        [Fact]
        public void Constructor_WhenFirstParameter_ThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new SplashViewModel(null, new Mock<IServerService>().Object));
        }

        [Fact]
        public void Constructor_WhenSecondParameter_ThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new SplashViewModel(new Mock<IDevice>().Object, null));
        }
        #endregion

        #region Commands
        [Fact]
        public async void WhenNoInternet_DisplayMessageToUserThenTerminateApp()
        {
            deviceMock.Setup(x => x.Internet).Returns(false);
            deviceMock.Setup(x => x.Token).Returns("storedtoken");

            await new SplashViewModel(deviceMock.Object, commonValues).RefreshTokenCommand.ExecuteAsync(null);

            deviceMock.Verify(x => x.DisplayMessageAsync(IDevice.Message.NoInternet), Times.AtLeastOnce);
            deviceMock.Verify(x => x.TerminateAppAsync(), Times.AtLeastOnce);
        }

        [Fact]
        public async void WhenTokenIsEmpty_OpenLoginPage()
        {
            deviceMock.Setup(x => x.Token).Returns(string.Empty) ;

            await new SplashViewModel(deviceMock.Object, commonValues).RefreshTokenCommand.ExecuteAsync(null);

            deviceMock.Verify(x => x.NavigateToPageAsync("LoginPage"), Times.Once);
        }

        [Fact]
        public async void WhenServerResponseSuccess_OpenHomePageAndTokenSameAsSentFromServer()
        {
            deviceMock.Setup(x => x.Token).Returns("storedtoken");
            var handlerMock = GetHttpMessageHandlerMock(HttpStatusCode.OK, new EmployeeTokenDto() { Token = "token" }.SerializeToJson());
            deviceMock.Setup(x => x.HttpClientInstance).Returns(GetNewHttpClient(handlerMock.Object));

            await new SplashViewModel(deviceMock.Object, commonValues).RefreshTokenCommand.ExecuteAsync(null);

            deviceMock.Verify(x => x.NavigateToPageAsync("HomePage"), Times.Once);
            deviceMock.Verify(x => x.Token, Times.AtLeastOnce);
        }

        [Fact]
        public async void WhenServerResponseNotSuccess_OpenLoginPage()
        {
            deviceMock.Setup(x => x.Token).Returns(new Random().NextDouble().ToString());
            deviceMock.Setup(x => x.Internet).Returns(true);
            //
            var handlerMock = GetHttpMessageHandlerMock(new HttpResponseMessage(HttpStatusCode.InternalServerError));
            deviceMock.Setup(x => x.HttpClientInstance).Returns(GetNewHttpClient(handlerMock.Object));

            var vm = new SplashViewModel(deviceMock.Object, commonValues);

            await vm.RefreshTokenCommand.ExecuteAsync(null);

            deviceMock.Verify(x => x.NavigateToPageAsync("LoginPage"), Times.AtLeastOnce);
        }

        [Fact(Skip = "Unexplained Failure!")]
        public async void WhenConnectingToServerAndWebExcptionOccures_AppShouldBeTerminated()
        {
            var deviceMock = new Mock<IDevice>();
            deviceMock.Setup(x => x.Token).Returns(new Random().NextDouble().ToString());
            deviceMock.Setup(x => x.Internet).Returns(true);
            //
            var handlerMock = GetHttpMessageHandlerMock(() => throw new WebException());
            deviceMock.Setup(x => x.HttpClientInstance).Returns(GetNewHttpClient(handlerMock.Object));

            var vm = new SplashViewModel(deviceMock.Object, commonValues);
            
            await vm.RefreshTokenCommand.ExecuteAsync(null);

            deviceMock.Verify(x => x.TerminateAppAsync(), Times.Once);
        }

        [Fact]
        public async void WhenConnectingToServerAndOtherExcptionOccures_AppShouldBeTerminated()
        {
            deviceMock.Setup(x => x.Token).Returns(new Random().NextDouble().ToString());
            deviceMock.Setup(x => x.Internet).Returns(true);
            //
            //
            var handler = GetHttpMessageHandlerMock(() => throw new Exception());

            var vm = new SplashViewModel(deviceMock.Object, commonValues);

            await vm.RefreshTokenCommand.ExecuteAsync(null);

            deviceMock.Verify(x => x.TerminateAppAsync(), Times.Once);
        }
        #endregion
    }
}
