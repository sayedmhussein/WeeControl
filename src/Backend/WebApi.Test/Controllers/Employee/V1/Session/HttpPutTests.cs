using System;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using WeeControl.SharedKernel.Aggregates.Employee.DtosV1;
using WeeControl.SharedKernel.Common;
using WeeControl.SharedKernel.DtosV1;
using Xunit;

namespace WeeControl.Backend.WebApi.Test.Controllers.Employee.V1.Session
{
    public class HttpPutTests : IClassFixture<WebApplicationFactory<Startup>>, IDisposable
    {
        private readonly IHttpMessage httpMessage;
        private readonly IFunctionalAuthorization userToken;
        private HttpRequestMessage request;

        public HttpPutTests(WebApplicationFactory<Startup> factory)
        {
            var client = factory.CreateClient();

            httpMessage = new HttpMessage(client, typeof(FunctionalTestTemplate).Namespace);
            userToken = new FunctionalAuthorization(httpMessage);

            request = new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                Version = new Version("1.0"),
                RequestUri = httpMessage.GetUri(ApiRouteEnum.EmployeeSession)
            };
        }

        public void Dispose()
        {
            request = null;
        }

        [Fact]
        public async void WhenPostingInvalidDto_ResponseIsBadRequest()
        {
            request.Content = httpMessage.GetHttpContentAsJson(new RefreshLoginDto());

            var token = await userToken.GetTokenAsync("admin", "admin");
            var response = await httpMessage.GetResponseMessageAsync(request, token);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async void WhenPostingInvalidPayloadDto_ResponseIsBadRequest()
        {
            request.Content = httpMessage.GetHttpContentAsJson(new RequestDto<string>() { Payload = "" });

            var token = await userToken.GetTokenAsync("admin", "admin");
            var response = await httpMessage.GetResponseMessageAsync(request, token);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async void WhenRefreshingTokenWithoutValidToken_ReturnUnauthorized()
        {
            var response = await httpMessage.GetResponseMessageAsync(request);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async void WhenRefreshingTokenWithValidToken_ReturnOk()
        {
            request.Content = httpMessage.GetHttpContentAsJson(new RequestDto<RefreshLoginDto>() { DeviceId = httpMessage.DeviceId });

            var token = await userToken.GetTokenAsync("admin", "admin");
            var response = await httpMessage.GetResponseMessageAsync(request, token);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async void WhenRefreshingTokenWithValidTokenButDifferentDevice_ReturnForbidden()
        {
            request.Content = httpMessage.GetHttpContentAsJson(new RequestDto<RefreshLoginDto>() { DeviceId = "SomeOtherDevice" });

            var token = await userToken.GetTokenAsync("admin", "admin");
            var response = await httpMessage.GetResponseMessageAsync(request, token);

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }
    }
}
