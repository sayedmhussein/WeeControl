using System;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using MySystem.SharedKernel.EntityV1Dtos.Common;
using MySystem.SharedKernel.Enumerators;
using MySystem.SharedKernel.Enumerators.Common;
using MySystem.SharedKernel.Interfaces;
using MySystem.SharedKernel.Interfaces.Values;
using MySystem.SharedKernel.Services;
using MySystem.User.Employee.Enumerators;
using MySystem.User.Employee.Interfaces;
using MySystem.User.Employee.Services;
using MySystem.User.Employee.ViewModels;
using MySystem.Web.Api;
using Xunit;

namespace MySystem.User.Employee.Test.FunctionalTests.Employee
{
    public class CredentialTests : IClassFixture<WebApplicationFactory<Startup>>, IDisposable
    {
        private const string USERNAME = "admin";
        private const string PASSWORD = "admin";

        private ICommonValues sharedValues;
        private Mock<IDevice> deviceMock;

        private HttpClient client;
        private readonly string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        private IViewModelDependencyFactory dependencyFactory;

        public CredentialTests(WebApplicationFactory<Startup> factory)
        {
            sharedValues = new CommonValues();

            deviceMock = new Mock<IDevice>();
            deviceMock.SetupProperty(x => x.Token);
            deviceMock.Setup(x => x.Internet).Returns(true);
            deviceMock.Setup(x => x.Metadata).Returns(new RequestMetadata() { Device = "Somedevice" });

            client = factory.CreateClient();
            client.BaseAddress = new Uri(sharedValues.ApiRoute[ApiRouteEnum.Base]);
            client.DefaultRequestVersion = new Version(sharedValues.ApiRoute[ApiRouteEnum.Version]);

            dependencyFactory = new ViewModelDependencyFactory(client, deviceMock.Object, appDataPath);
        }

        public void Dispose()
        {
            deviceMock = null;
            client = null;
            dependencyFactory = null;
        }

        [Fact]
        public async void WhenLoginWithValidCredentails_OpenSplashPage()
        {
            var vm = new LoginViewModel(dependencyFactory, sharedValues)
            {
                Username = USERNAME,
                Password = PASSWORD
            };

            await vm.LoginCommand.ExecuteAsync(null);

            deviceMock.Verify(x => x.NavigateToPageAsync(nameof(ApplicationPageEnum.SplashPage)), Times.Once);
            Assert.NotEmpty(deviceMock.Object.Token);
        }

        [Fact]
        public async void WhenLoginWithInValidCredentails_DonNotOpenHomePage()
        {
            var vm = new LoginViewModel(dependencyFactory, sharedValues)
            {
                Username = "admin1",
                Password = "admin"
            };

            await vm.LoginCommand.ExecuteAsync(null);

            deviceMock.Verify(x => x.NavigateToPageAsync(nameof(ApplicationPageEnum.HomePage)), Times.Never);
        }

        [Fact]
        public async void WhenSplashFindCorrectToken_OpenHomePage()
        {
            var loginVm = new LoginViewModel(dependencyFactory, sharedValues)
            {
                Username = USERNAME,
                Password = PASSWORD
            };
            await loginVm.LoginCommand.ExecuteAsync(null);

            var splashVm = new SplashViewModel(dependencyFactory, sharedValues);
            await splashVm.RefreshTokenCommand.ExecuteAsync(null);

            deviceMock.Verify(x => x.NavigateToPageAsync(nameof(ApplicationPageEnum.HomePage)), Times.Once);
            deviceMock.Verify(x => x.NavigateToPageAsync(nameof(ApplicationPageEnum.SplashPage)), Times.Once);
            deviceMock.Verify(x => x.NavigateToPageAsync(nameof(ApplicationPageEnum.LoginPage)), Times.Never);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("blabla")]
        [InlineData("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c")]
        public async void WhenSplashFindInvalidToken_OpenLoginPage(string token)
        {
            deviceMock.Setup(x => x.Token).Returns(token);
            var vm = new SplashViewModel(dependencyFactory, sharedValues);

            await vm.RefreshTokenCommand.ExecuteAsync(null);

            deviceMock.Verify(x => x.NavigateToPageAsync(nameof(ApplicationPageEnum.HomePage)), Times.Never);
            deviceMock.Verify(x => x.NavigateToPageAsync(nameof(ApplicationPageEnum.LoginPage)), Times.Once);
        }
    }
}
