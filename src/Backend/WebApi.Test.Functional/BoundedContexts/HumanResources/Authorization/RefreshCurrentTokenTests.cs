using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WeeControl.Backend.WebApi.Test.Functional.TestHelpers;
using WeeControl.Common.SharedKernel;
using WeeControl.Common.SharedKernel.Obsolute.Common;
using WeeControl.Common.SharedKernel.Obsolute.Employee;
using Xunit;

namespace WeeControl.Backend.WebApi.Test.Functional.BoundedContexts.HumanResources.Authorization
{
    public class RefreshCurrentTokenTests : IClassFixture<CustomWebApplicationFactory<Startup>>, ITestsRequireAuthentication
    {
        #region static
        private const string Route = ApiRouteLink.HumanResources.Authorization.RequestRefreshToken.Absolute;
        private static readonly HttpMethod Method = ApiRouteLink.HumanResources.Authorization.RequestRefreshToken.Method;
        private const string Version = ApiRouteLink.HumanResources.Authorization.RequestRefreshToken.Version;

        public static async Task<string> GetRefreshedTokenAsync(CustomWebApplicationFactory<Startup> factory, string device)
        {
            var token1 = await RequestNewTokenTests.GetNewTokenAsync(factory, device);
            
            var test = new FunctionalTest(factory, device, Method, Version);
            test.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token1);
            
            var content = test.GetHttpContentAsJson(new RequestDto()
            {
                DeviceId = test.DeviceId,
            });

            var response = await test.GetResponseMessageAsync(new Uri(Route), content);
            response.EnsureSuccessStatusCode();
            var tokenDto = await response.Content.ReadFromJsonAsync<ResponseDto<EmployeeTokenDto>>();

            return tokenDto?.Payload.Token;
        }
        #endregion
        
        private readonly CustomWebApplicationFactory<Startup> factory;
        private readonly IFunctionalTest test;
        private readonly Uri routeUri;
        
        public RefreshCurrentTokenTests(CustomWebApplicationFactory<Startup> factory)
        {
            this.factory = factory;

            HttpRequestMessage message = new()
            {
                RequestUri = new Uri(ApiRouteLink.HumanResources.Authorization.RequestRefreshToken.Absolute),
                Version = new Version(ApiRouteLink.HumanResources.Authorization.RequestRefreshToken.Version),
                Method = ApiRouteLink.HumanResources.Authorization.RequestRefreshToken.Method
            };
            
            test = new FunctionalTest(factory,nameof(RefreshCurrentTokenTests), Method, Version);
            routeUri = new Uri(Route);
        }
        
        [Fact]
        public async void WhenSendingValidRequest_HttpResponseIsSuccessCode()
        {
            var token = await GetRefreshedTokenAsync(factory, nameof(RefreshCurrentTokenTests));

            Assert.NotEmpty(token);
        }
        
        public void WhenUnAuthenticatedUser_HttpResponseIsUnauthorized()
        {
            throw new NotImplementedException();
        }

        public void WhenAuthenticatedButInvalidRequest_HttpResponseIsBadRequest()
        {
            throw new NotImplementedException();
        }

        public void WhenAuthenticatedButNotAuthorized_HttpResponseIsForbidden()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public async void WhenPostingInvalidDto_ResponseIsBadRequest()
        {
            var content = test.GetHttpContentAsJson(new RefreshLoginDto());

            var token = await GetRefreshedTokenAsync(factory, nameof(RefreshCurrentTokenTests));
            var response = await test.GetResponseMessageAsync(routeUri, content, token);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async void WhenPostingInvalidPayloadDto_ResponseIsBadRequest()
        {
            var content = test.GetHttpContentAsJson(new RequestDto<string>() { Payload = "" });

            var token = await GetRefreshedTokenAsync(factory, nameof(RefreshCurrentTokenTests));
            var response = await test.GetResponseMessageAsync(routeUri, content, token);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async void WhenRefreshingTokenWithoutValidToken_ReturnUnauthorized()
        {
            var content = test.GetHttpContentAsJson(new RequestDto<RefreshLoginDto>() { DeviceId = test.DeviceId });

            var response = await test.GetResponseMessageAsync(routeUri, content);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async void WhenRefreshingTokenWithValidToken_ReturnOk()
        {
            var content = test.GetHttpContentAsJson(new RequestDto<RefreshLoginDto>() { DeviceId = test.DeviceId });
            
            var response = await test.GetResponseMessageAsync(routeUri, content, "token");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async void WhenRefreshingTokenWithValidTokenButDifferentDevice_ReturnForbidden()
        {
            var content = test.GetHttpContentAsJson(new RequestDto<RefreshLoginDto>() { DeviceId = "SomeOtherDevice" });
            
            var response = await test.GetResponseMessageAsync(routeUri, content, "token");

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }
    }
}
