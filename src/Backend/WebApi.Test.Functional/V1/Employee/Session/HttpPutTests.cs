using System;
using System.Net;
using System.Net.Http;
using WeeControl.SharedKernel.EntityGroup.Employee.DtosV1;
using WeeControl.SharedKernel.Common;
using WeeControl.SharedKernel.DtosV1;
using Xunit;

namespace WeeControl.Backend.WebApi.Test.Functional.V1.Employee.Session
{
    public class HttpPutTests :
        BaseFunctionalTest,
        IClassFixture<CustomWebApplicationFactory<Startup>>,
        IDisposable
    {
        public HttpPutTests(CustomWebApplicationFactory<Startup> factory) :
            base(factory, HttpMethod.Put, typeof(HttpPutTests).Namespace)
        {
            ServerUri = GetUri(ApiRouteEnum.EmployeeSession);
        }

        [Fact]
        public async void WhenPostingInvalidDto_ResponseIsBadRequest()
        {
            var content = GetHttpContentAsJson(new RefreshLoginDto());

            var token = await authorization.GetTokenAsync("admin", "admin");
            var response = await GetResponseMessageAsync(ServerUri, content, token);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async void WhenPostingInvalidPayloadDto_ResponseIsBadRequest()
        {
            var content = GetHttpContentAsJson(new RequestDto<string>() { Payload = "" });

            var token = await authorization.GetTokenAsync("admin", "admin");
            var response = await GetResponseMessageAsync(ServerUri, content, token);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async void WhenRefreshingTokenWithoutValidToken_ReturnUnauthorized()
        {
            var content = GetHttpContentAsJson(new RequestDto<RefreshLoginDto>() { DeviceId = DeviceId });

            var response = await GetResponseMessageAsync(ServerUri, content);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async void WhenRefreshingTokenWithValidToken_ReturnOk()
        {
            var content = GetHttpContentAsJson(new RequestDto<RefreshLoginDto>() { DeviceId = DeviceId });

            var token = await authorization.GetTokenAsync("admin", "admin");
            var response = await GetResponseMessageAsync(ServerUri, content, token);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async void WhenRefreshingTokenWithValidTokenButDifferentDevice_ReturnForbidden()
        {
            var content = GetHttpContentAsJson(new RequestDto<RefreshLoginDto>() { DeviceId = "SomeOtherDevice" });

            var token = await authorization.GetTokenAsync("admin", "admin");
            var response = await GetResponseMessageAsync(ServerUri, content, token);

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }
    }
}
