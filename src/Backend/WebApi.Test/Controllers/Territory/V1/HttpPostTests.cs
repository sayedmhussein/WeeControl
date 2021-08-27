using System;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using WeeControl.SharedKernel.Aggregates.Territory.DtosV1;
using WeeControl.SharedKernel.Common;
using WeeControl.SharedKernel.DtosV1;
using Xunit;

namespace WeeControl.Backend.WebApi.Test.Controllers.Territory.V1
{
    public class HttpPostTests :
        IClassFixture<WebApplicationFactory<Startup>>,
        IDisposable
    {
        private readonly IHttpMessage httpMessage;
        private readonly IFunctionalAuthorization userToken;
        private HttpRequestMessage request;

        public HttpPostTests(WebApplicationFactory<Startup> factory)
        {
            var client = factory.CreateClient();

            httpMessage = new HttpMessage(client, typeof(FunctionalTestTemplate).Namespace);
            userToken = new FunctionalAuthorization(httpMessage);

            var requestDto = new RequestDto<TerritoryDto>()
            {
                DeviceId = httpMessage.DeviceId,
                Payload = new TerritoryDto()
                {
                    Name = new Random().NextDouble().ToString(),
                    CountryId = "TST"
                }
            };

            request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                Version = new Version("1.0"),
                RequestUri = httpMessage.GetUri(ApiRouteEnum.Territory),
                Content = httpMessage.GetHttpContentAsJson(requestDto)
            };
        }

        public void Dispose()
        {
            request = null;
        }

        [Fact]
        public async void WhenWithoutToken_ReturnUnAuthorized()
        {
            var response = await httpMessage.GetResponseMessageAsync(request);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async void WhenPosting_ReturnCreated()
        {
            var token = await userToken.GetTokenAsync("admin", "admin");

            var response = await httpMessage.GetResponseMessageAsync(request, token);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async void WhenPostingSameCountryAndNameTwice_ReturnConflict()
        {
            var token = await userToken.GetTokenAsync("admin", "admin");
            var request1 = request;
            var request2 = await httpMessage.CloneAsync(request1);

            var response = await httpMessage.GetResponseMessageAsync(request1, token);
            response.EnsureSuccessStatusCode();

            var response2 = await httpMessage.GetResponseMessageAsync(request2, token);

            Assert.Equal(HttpStatusCode.Conflict, response2.StatusCode);
        }

        [Fact]
        public async void WhenPostingInvalidContent_ReturnBadRequest()
        {
            var token = await userToken.GetTokenAsync("admin", "admin");

            request.Content = httpMessage.GetHttpContentAsJson(new TerritoryDto());
            var response = await httpMessage.GetResponseMessageAsync(request, token);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
