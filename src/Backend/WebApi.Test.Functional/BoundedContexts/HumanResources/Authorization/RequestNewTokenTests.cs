using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Moq;
using WeeControl.Backend.WebApi.Test.Functional.TestHelpers;
using WeeControl.Common.SharedKernel;
using WeeControl.Common.SharedKernel.BoundedContexts.HumanResources.Authentication;
using WeeControl.Common.SharedKernel.BoundedContexts.HumanResources.ClientSideServices;
using WeeControl.Common.SharedKernel.BoundedContexts.Shared;
using WeeControl.Common.SharedKernel.Interfaces;
using WeeControl.Common.SharedKernel.Obsolutes.Dtos;
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

        public static async Task<string> GetNewTokenAsync2(CustomWebApplicationFactory<Startup> factory, string device)
        {
            HttpRequestMessage message = new()
            {
                RequestUri = new Uri(ApiRouteLink.HumanResources.Authorization.RequestNewToken.Absolute),
                Version = new Version(ApiRouteLink.HumanResources.Authorization.RequestNewToken.Version),
                Method = ApiRouteLink.HumanResources.Authorization.RequestNewToken.Method,
                Content = FunctionalTestService.GetHttpContentAsJson(new RequestDto<RequestNewTokenDto>(device, new RequestNewTokenDto("admin", "admin")))
            };
            
            var test = new FunctionalTestService(factory);
            
            var response = await test.GetResponseMessageAsync(message);
            response.EnsureSuccessStatusCode();
            var tokenDto = await response.Content.ReadFromJsonAsync<ResponseDto<EmployeeTokenDto>>();

            return tokenDto?.Payload.Token;
        }
        #endregion
        
        private readonly CustomWebApplicationFactory<Startup> factory;
        private readonly IFunctionalTestService testService;
        private readonly string device;

        public RequestNewTokenTests(CustomWebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
            device = nameof(RequestNewTokenTests);
            testService = new FunctionalTestService(factory);
        }

        [Fact]
        public async void WhenSendingInvalidRequest_HttpResponseIsBadRequest()
        {
            Mock<IClientDevice> deviceMock = new();
            deviceMock.SetupAllProperties();
            deviceMock.Setup(x => x.DeviceId).Returns(device);

            IAuthenticationService service = new AuthenticationService(factory.CreateClient(), deviceMock.Object);
            var response2 = await service.RequestNewToken(null);
            
            Assert.Equal(HttpStatusCode.BadRequest, response2.HttpStatuesCode);
            
            
            
            HttpRequestMessage defaultRequestMessage = new()
            {
                RequestUri = new Uri(ApiRouteLink.HumanResources.Authorization.RequestNewToken.Absolute),
                Version = new Version(ApiRouteLink.HumanResources.Authorization.RequestNewToken.Version),
                Method = ApiRouteLink.HumanResources.Authorization.RequestNewToken.Method,
                Content = FunctionalTestService.GetHttpContentAsJson(new RequestDto<string>("InvalidPayload", device))
            };

            var response = await testService.GetResponseMessageAsync(defaultRequestMessage);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async void WhenSendingValidRequest2_HttpResponseIsSuccessCode()
        {
            var token = await GetNewTokenAsync2(factory, typeof(RequestNewTokenTests).Namespace);
            
            Assert.NotEmpty(token);
        }
        
        [Fact]
        public async void WhenSendingValidRequest_HttpResponseIsSuccessCode()
        {
            var token = await GetNewTokenAsync(factory, typeof(RequestNewTokenTests).Namespace);
            
            Assert.NotEmpty(token);
        }
    }
}