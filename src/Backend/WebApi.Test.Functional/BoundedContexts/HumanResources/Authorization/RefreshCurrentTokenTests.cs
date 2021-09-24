using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WeeControl.Backend.WebApi.Test.Functional.TestHelpers;
using WeeControl.Common.SharedKernel;
using WeeControl.Common.SharedKernel.BoundedContextDtos.Shared;
using WeeControl.Common.SharedKernel.Obsolutes.Dtos;
using Xunit;

namespace WeeControl.Backend.WebApi.Test.Functional.BoundedContexts.HumanResources.Authorization
{
    public class RefreshCurrentTokenTests : IClassFixture<CustomWebApplicationFactory<Startup>>, ITestsRequireAuthentication
    {
        #region static
        public static async Task<string> GetRefreshedTokenAsync(CustomWebApplicationFactory<Startup> factory, string device)
        {
            HttpRequestMessage message = new()
            {
                RequestUri = new Uri(ApiRouteLink.HumanResources.Authorization.RequestRefreshToken.Absolute),
                Version = new Version(ApiRouteLink.HumanResources.Authorization.RequestRefreshToken.Version),
                Method = ApiRouteLink.HumanResources.Authorization.RequestRefreshToken.Method,
                Content = FunctionalTestService.GetHttpContentAsJson(new RequestDto(device))
            };
            
            var test = new FunctionalTestService(factory);
            
            var token = await RequestNewTokenTests.GetNewTokenAsync(factory, device);
            
            var response = await test.GetResponseMessageAsync(message, token);
            response.EnsureSuccessStatusCode();
            var tokenDto = await response.Content.ReadFromJsonAsync<ResponseDto<EmployeeTokenDto>>();

            return tokenDto?.Payload.Token;
        }
        #endregion
        
        private readonly CustomWebApplicationFactory<Startup> factory;
        private readonly IFunctionalTestService testService;
        private readonly string device;

        public RefreshCurrentTokenTests(CustomWebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
            device = nameof(RefreshCurrentTokenTests);
            testService = new FunctionalTestService(factory);
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
            HttpRequestMessage message = new()
            {
                RequestUri = new Uri(ApiRouteLink.HumanResources.Authorization.RequestRefreshToken.Absolute),
                Version = new Version(ApiRouteLink.HumanResources.Authorization.RequestRefreshToken.Version),
                Method = ApiRouteLink.HumanResources.Authorization.RequestRefreshToken.Method,
                Content = FunctionalTestService.GetHttpContentAsJson(new RequestDto("Other device!"))
            };
            
            var response = await testService.GetResponseMessageAsync(message);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async void WhenAuthenticatedButInvalidRequest_HttpResponseIsBadRequest()
        {
            var token = await RequestNewTokenTests.GetNewTokenAsync(factory, device);
            HttpRequestMessage message = new()
            {
                RequestUri = new Uri(ApiRouteLink.HumanResources.Authorization.RequestRefreshToken.Absolute),
                Version = new Version(ApiRouteLink.HumanResources.Authorization.RequestRefreshToken.Version),
                Method = ApiRouteLink.HumanResources.Authorization.RequestRefreshToken.Method,
                Content = FunctionalTestService.GetHttpContentAsJson(new {})
            };
            
            var response = await testService.GetResponseMessageAsync(message, token);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async void WhenAuthenticatedButNotAuthorized_HttpResponseIsForbidden()
        {
            //When different device...
            var token = await RequestNewTokenTests.GetNewTokenAsync(factory, device);
            HttpRequestMessage message = new()
            {
                RequestUri = new Uri(ApiRouteLink.HumanResources.Authorization.RequestRefreshToken.Absolute),
                Version = new Version(ApiRouteLink.HumanResources.Authorization.RequestRefreshToken.Version),
                Method = ApiRouteLink.HumanResources.Authorization.RequestRefreshToken.Method,
                Content = FunctionalTestService.GetHttpContentAsJson(new RequestDto("Other device!"))
            };
            
            var response = await testService.GetResponseMessageAsync(message, token);

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }
    }
}
