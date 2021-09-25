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
    public class RequestNewTokenTests : IClassFixture<CustomWebApplicationFactory<Startup>>, ITestsNotRequireAuthentication
    {
        #region static
        public static async Task<string> GetNewTokenAsync(CustomWebApplicationFactory<Startup> factory, string device)
        {
            var token = string.Empty;
            
            Mock<IClientDevice> deviceMock = new();
            deviceMock.SetupAllProperties();
            deviceMock.Setup(x => x.DeviceId).Returns(device);
            deviceMock.Setup(x => x.SaveTokenAsync(It.IsAny<string>())).Callback<string>(y => token = y);
            
            IAuthenticationService service = new AuthenticationService(factory.CreateClient(), deviceMock.Object);
            var response = await service.RequestNewToken(new RequestNewTokenDto("admin", "admin"));


            return token;
        }
        #endregion
        
        private readonly CustomWebApplicationFactory<Startup> factory;
        private readonly string device;

        public RequestNewTokenTests(CustomWebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
            device = nameof(RequestNewTokenTests);
        }

        [Fact]
        public async void WhenSendingInvalidRequest_HttpResponseIsBadRequest()
        {
            Mock<IClientDevice> deviceMock = new();
            deviceMock.SetupAllProperties();
            deviceMock.Setup(x => x.DeviceId).Returns(device);

            IAuthenticationService service = new AuthenticationService(factory.CreateClient(), deviceMock.Object);
            var response2 = await service.RequestNewToken(null);
            
            Assert.Equal(HttpStatusCode.BadRequest, response2.StatuesCode);
        }

        [Fact]
        public async void WhenSendingValidRequest_HttpResponseIsSuccessCode()
        {
            var token = await GetNewTokenAsync(factory, typeof(RequestNewTokenTests).Namespace);
            
            Assert.NotEmpty(token);
        }
    }
}