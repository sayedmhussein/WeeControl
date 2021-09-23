using System;
using System.Net;
using System.Net.Http;
using WeeControl.Backend.WebApi.Test.Functional.TestHelpers;
using WeeControl.Common.SharedKernel.Obsolute.Common;
using WeeControl.Common.SharedKernel.Obsolute.Employee;
using WeeControl.Common.SharedKernel.Routing;
using Xunit;

namespace WeeControl.Backend.WebApi.Test.Functional.BoundedContexts.HumanResources
{
    public class HttpPutTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> factory;
        private readonly IFunctionalTest test;
        private readonly IFunctionalAuthorization authorization;
        private readonly Uri routeUri;
        
        public HttpPutTests(CustomWebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
            test = new FunctionalTest(factory, HttpMethod.Put, typeof(HttpPutTests).Namespace);
            authorization = new FunctionalAuthorization(test);
            routeUri = test.GetUri(ApiRouteEnum.EmployeeSession);
        }

        [Fact]
        public async void WhenPostingInvalidDto_ResponseIsBadRequest()
        {
            var content = test.GetHttpContentAsJson(new RefreshLoginDto());

            var token = await authorization.GetTokenAsync("admin", "admin");
            var response = await test.GetResponseMessageAsync(routeUri, content, token);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async void WhenPostingInvalidPayloadDto_ResponseIsBadRequest()
        {
            var content = test.GetHttpContentAsJson(new RequestDto<string>() { Payload = "" });

            var token = await authorization.GetTokenAsync("admin", "admin");
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

            var token = await authorization.GetTokenAsync("admin", "admin");
            var response = await test.GetResponseMessageAsync(routeUri, content, token);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async void WhenRefreshingTokenWithValidTokenButDifferentDevice_ReturnForbidden()
        {
            var content = test.GetHttpContentAsJson(new RequestDto<RefreshLoginDto>() { DeviceId = "SomeOtherDevice" });

            var token = await authorization.GetTokenAsync("admin", "admin");
            var response = await test.GetResponseMessageAsync(routeUri, content, token);

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }
    }
}
