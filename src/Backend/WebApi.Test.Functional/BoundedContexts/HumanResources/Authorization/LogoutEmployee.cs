using System;
using System.Net;
using Moq;
using WeeControl.Backend.WebApi.Test.Functional.TestHelpers;
using WeeControl.Common.SharedKernel.BoundedContexts.HumanResources.ClientSideServices;
using WeeControl.Common.SharedKernel.Interfaces;
using Xunit;

namespace WeeControl.Backend.WebApi.Test.Functional.BoundedContexts.HumanResources.Authorization
{
    public class LogoutEmployee : IClassFixture<CustomWebApplicationFactory<Startup>>, IDisposable, ITestsRequireAuthentication
    {
        private readonly CustomWebApplicationFactory<Startup> factory;
        private readonly string device;
        private Mock<IClientDevice> clientDeviceMock;
        
        public LogoutEmployee(CustomWebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
            device = nameof(LogoutEmployee);
            
            clientDeviceMock = new Mock<IClientDevice>();
            clientDeviceMock.SetupAllProperties();
            clientDeviceMock.Setup(x => x.DeviceId).Returns(device);
        }

        public void Dispose()
        {
            clientDeviceMock = null;
        }

        [Fact]
        public async void WhenSendingValidRequest_HttpResponseIsSuccessCode()
        {
            var token = await RefreshCurrentTokenTests.GetRefreshedTokenAsync(factory, device);
            clientDeviceMock.Setup(x => x.GetTokenAsync()).ReturnsAsync(token);
            var service = new AuthenticationService(factory.CreateClient(), clientDeviceMock.Object);

            var response = await service.Logout();
            
            Assert.Equal(HttpStatusCode.OK, response.StatuesCode);
        }

        [Fact]
        public async void WhenUnAuthenticatedUser_HttpResponseIsUnauthorized()
        {
            var service = new AuthenticationService(factory.CreateClient(), clientDeviceMock.Object);

            var response = await service.Logout();
            
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatuesCode);
        }

        [Fact]
        public async void WhenAuthenticatedButNotAuthorized_HttpResponseIsForbidden()
        {
            var token = await RefreshCurrentTokenTests.GetRefreshedTokenAsync(factory, device);
            clientDeviceMock.Setup(x => x.GetTokenAsync()).ReturnsAsync(token);
            var service = new AuthenticationService(factory.CreateClient(), clientDeviceMock.Object);

            var response1 = await service.Logout();
            Assert.Equal(HttpStatusCode.OK, response1.StatuesCode);
            
            var response2 = await service.Logout();
            Assert.Equal(HttpStatusCode.Forbidden, response2.StatuesCode);
        }
    }
}