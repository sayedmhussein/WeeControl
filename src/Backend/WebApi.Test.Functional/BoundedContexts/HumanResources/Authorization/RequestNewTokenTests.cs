using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WeeControl.Backend.WebApi.Test.Functional.TestHelpers;
using WeeControl.Common.SharedKernel;
using WeeControl.Common.SharedKernel.BoundedContextDtos.HumanResources.Authorization;
using WeeControl.Common.SharedKernel.BoundedContextDtos.Shared;
using WeeControl.Common.SharedKernel.Obsolutes.Dtos;
using Xunit;

namespace WeeControl.Backend.WebApi.Test.Functional.BoundedContexts.HumanResources.Authorization
{
    public class RequestNewTokenTests : IClassFixture<CustomWebApplicationFactory<Startup>>, ITestsNotRequireAuthentication
    {
        #region static
        public static async Task<string> GetNewTokenAsync(CustomWebApplicationFactory<Startup> factory, string device)
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
        public async void WhenSendingValidRequest_HttpResponseIsSuccessCode()
        {
            var token = await GetNewTokenAsync(factory, typeof(RequestNewTokenTests).Namespace);
            
            Assert.NotEmpty(token);
        }
    }
}