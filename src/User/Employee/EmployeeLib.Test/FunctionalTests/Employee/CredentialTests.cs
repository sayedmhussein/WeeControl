using System;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using MySystem.SharedKernel.EntityV1Dtos.Common;
using MySystem.SharedKernel.Enumerators;
using MySystem.SharedKernel.Interfaces;
using MySystem.SharedKernel.Services;
using MySystem.User.Employee.Interfaces;
using MySystem.User.Employee.Services;
using MySystem.User.Employee.ViewModels;
using MySystem.Web.Api;
using Xunit;

namespace MySystem.User.Employee.Test.FunctionalTests.Employee
{
    public class CredentialTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> factory;

        public CredentialTests(WebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
        }

        [Fact]
        public async void WhenLoginWithValidCredentails_GetTokenString()
        {
            ISharedValues sharedValues = new SharedValues();

            var client = factory.CreateClient();
            client.BaseAddress = new Uri(sharedValues.ApiRoute[ApiRouteEnum.Base]);
            client.DefaultRequestVersion = new Version(sharedValues.ApiRoute[ApiRouteEnum.Version]);

            var deviceMock = new Mock<IDevice>();
            deviceMock.SetupProperty(x => x.Token);
            deviceMock.Setup(x => x.Internet).Returns(true);
            deviceMock.Setup(x => x.Metadata).Returns(new RequestMetadata() { Device = "Somedevice" });
            
            var dependencyFactory = new ViewModelDependencyFactory(deviceMock.Object, sharedValues, null, client);

            var vm = new LoginViewModel(dependencyFactory);
            vm.Username = "admin";
            vm.Password = "admin";

            await vm.LoginCommand.ExecuteAsync(null);

            deviceMock.Verify(x => x.NavigateToPageAsync("HomePage"), Times.Once);
            Assert.NotEmpty(deviceMock.Object.Token);
        }
    }
}
