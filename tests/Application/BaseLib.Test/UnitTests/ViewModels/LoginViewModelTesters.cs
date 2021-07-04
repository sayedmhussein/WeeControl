using System;
using Moq;
using WeeControl.Applications.BaseLib.Interfaces;
using WeeControl.Applications.BaseLib.ViewModels;
using WeeControl.SharedKernel.BasicSchemas.Common.Extensions;
using WeeControl.SharedKernel.BasicSchemas.Common.Interfaces;
using WeeControl.SharedKernel.BasicSchemas.Employee.DtosV1;
using Xunit;

namespace WeeControl.User.Employee.Test.UnitTests.ViewModels
{
    public class LoginViewModelTesters : ViewModelTestBase
    {
        #region Constructor
        [Fact]
        public void Constructor_WhenFirstParameter_ThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new LoginViewModel(null, new Mock<IApiDicts>().Object));
        }

        [Fact]
        public void Constructor_WhenNullSecondParameter_ThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new LoginViewModel(new Mock<IDevice>().Object, null));
        }
        #endregion

        #region Commands
        [Fact]
        public async void LoginCommand_WhenEmptyUsername_ThrowInvalidArgumentException()
        {
            var service = deviceMock.Object;

            var vm = new LoginViewModel(service, commonValues)
            {
                Username = string.Empty,
                Password = "SomePassword"
            };

            await Assert.ThrowsAsync<ArgumentException>(async () => await vm.LoginCommand.ExecuteAsync(null));
        }

        [Fact]
        public async void LoginCommand_WhenNullPassword_ThrowInvalidArgumentException()
        {
            var service = deviceMock.Object;

            var vm = new LoginViewModel(service, commonValues)
            {
                Username = "SomeUsername",
                Password = null
            };

            await Assert.ThrowsAsync<ArgumentException>(async () => await vm.LoginCommand.ExecuteAsync(null));
        }

        [Fact]
        public async void LoginCommand_WhenNoInternet_DisplayMessage()
        {
            deviceMock.Setup(x => x.Internet).Returns(false);
            var handler = GetHttpMessageHandlerMock(System.Net.HttpStatusCode.OK, new EmployeeTokenDto() { Token = "token" }.SerializeToJson());
            //
            deviceMock.Setup(x => x.HttpClientInstance).Returns(new System.Net.Http.HttpClient(handler.Object));
            

            var vm = new LoginViewModel(deviceMock.Object, commonValues)
            {
                Username = "username",
                Password = "password"
            };

            await vm.LoginCommand.ExecuteAsync(new object());

            deviceMock.Verify(x => x.DisplayMessageAsync(IDevice.Message.NoInternet), Times.Once);
        }

        [Fact]
        public async void LoginCommand_WhenValidCredentials_OpenSplashPage()
        {
            var handler = GetHttpMessageHandlerMock(System.Net.HttpStatusCode.OK, new EmployeeTokenDto() { Token = "token" }.SerializeToJson());
            deviceMock.Setup(x => x.HttpClientInstance).Returns(GetNewHttpClient(handler.Object));

            var vm = new LoginViewModel(deviceMock.Object, commonValues)
            {
                Username = "username",
                Password = "password"
            };

            await vm.LoginCommand.ExecuteAsync(new object());

            deviceMock.Verify(x => x.NavigateToPageAsync("SplashPage"), Times.Once);
        }

        [Fact]
        public async void LoginCommand_WhenInvalidCredentials_DisplayMessageAndNeverOpenSplashPage()
        {
            var handler = GetHttpMessageHandlerMock(System.Net.HttpStatusCode.NotFound, new EmployeeTokenDto() { Token = "token" }.SerializeToJson());
            deviceMock.Setup(x => x.HttpClientInstance).Returns(GetNewHttpClient(handler.Object));

            var vm = new LoginViewModel(deviceMock.Object, commonValues)
            {
                Username = "admin",
                Password = "admin"
            };

            await vm.LoginCommand.ExecuteAsync(new object());

            deviceMock.Verify(x => x.NavigateToPageAsync("SplashPage"), Times.Never);
            deviceMock.Verify(x => x.DisplayMessageAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        
        #endregion
    }
}
