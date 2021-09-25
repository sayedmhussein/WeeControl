using System;
using System.Net;
using System.Threading.Tasks;
using Moq;
using WeeControl.Backend.WebApi.Test.Functional.TestHelpers;
using WeeControl.Common.SharedKernel.BoundedContexts.HumanResources.Authentication;
using WeeControl.Common.SharedKernel.BoundedContexts.HumanResources.ClientSideServices;
using WeeControl.Common.SharedKernel.Interfaces;
using Xunit;

namespace WeeControl.Backend.WebApi.Test.Functional.BoundedContexts.HumanResources.Authorization
{
    public class RefreshCurrentTokenTests : IClassFixture<CustomWebApplicationFactory<Startup>>, IDisposable, ITestsRequireAuthentication
    {
        #region static
        public static async Task<string> GetRefreshedTokenAsync(CustomWebApplicationFactory<Startup> factory,
            string device)
        {
            var token1 = await RequestNewTokenTests.GetNewTokenAsync(factory, device);
            var token2 = string.Empty;
            
            Mock<IClientDevice> deviceMock = new();
            deviceMock.SetupAllProperties();
            deviceMock.Setup(x => x.DeviceId).Returns(device);
            deviceMock.Setup(x => x.GetTokenAsync()).ReturnsAsync(token1);
            deviceMock.Setup(x => x.SaveTokenAsync(It.IsAny<string>())).Callback<string>(y => token2 = y);
            
            IAuthenticationService service = new AuthenticationService(factory.CreateClient(), deviceMock.Object);
            var response = await service.RefreshCurrentToken();

            return token2;
        }
        #endregion
        
        private readonly CustomWebApplicationFactory<Startup> factory;
        private readonly string device;
        private Mock<IClientDevice> clientDeviceMock;

        public RefreshCurrentTokenTests(CustomWebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
            device = nameof(RefreshCurrentTokenTests);
            
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
            var token = await GetRefreshedTokenAsync(factory, nameof(RefreshCurrentTokenTests));

            Assert.NotEmpty(token);
        }
        
        [Fact]
        public async void WhenUnAuthenticatedUser_HttpResponseIsUnauthorized()
        {
            IAuthenticationService service = new AuthenticationService(factory.CreateClient(), clientDeviceMock.Object);
            var response = await service.RefreshCurrentToken();

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatuesCode);
        }

        [Fact]
        public async void WhenAuthenticatedButNotAuthorized_HttpResponseIsForbidden()
        {
            //When different device...
            var token = await RequestNewTokenTests.GetNewTokenAsync(factory, device);
            
            clientDeviceMock.Setup(x => x.DeviceId).Returns("Other Device");
            clientDeviceMock.Setup(x => x.GetTokenAsync()).ReturnsAsync(token);
            
            var service = new AuthenticationService(factory.CreateClient(), clientDeviceMock.Object);
            var response = await service.RefreshCurrentToken();

            Assert.Equal(HttpStatusCode.Forbidden, response.StatuesCode);
        }
    }
}
