using System;
using System.Net.Http;
using Moq;
using MySystem.SharedKernel.EntityV1Dtos.Employee;
using MySystem.SharedKernel.ExtensionMethods;
using MySystem.User.Employee.Interfaces;
using MySystem.User.Employee.Services;
using MySystem.User.Employee.ViewModels;
using Xunit;

namespace MySystem.User.Employee.Test.UnitTests.ViewModels
{
    public class LoginViewModelTesters : ViewModelTestBase
    {
        #region Constructor
        [Fact]
        public void Constructor_WhenNullService_ThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new LoginViewModel(null));
        }
        #endregion

        #region Commands
        [Fact]
        public async void LoginCommand_WhenEmptyUsername_ThrowInvalidArgumentException()
        {
            var service = clientServicesMock.Object;

            var vm = new LoginViewModel(service)
            {
                Username = string.Empty,
                Password = "SomePassword"
            };

            await Assert.ThrowsAsync<ArgumentException>(async () => await vm.LoginCommand.ExecuteAsync(null));
        }

        [Fact]
        public async void LoginCommand_WhenNullPassword_ThrowInvalidArgumentException()
        {
            var service = clientServicesMock.Object;

            var vm = new LoginViewModel(service)
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
            clientServicesMock.Setup(x => x.HttpClientInstance).Returns(new System.Net.Http.HttpClient(handler.Object));
            

            var vm = new LoginViewModel(clientServicesMock.Object)
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
            clientServicesMock.Setup(x => x.HttpClientInstance).Returns(GetNewHttpClient(handler.Object));

            var vm = new LoginViewModel(clientServicesMock.Object)
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
            clientServicesMock.Setup(x => x.HttpClientInstance).Returns(GetNewHttpClient(handler.Object));

            var vm = new LoginViewModel(clientServicesMock.Object)
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
