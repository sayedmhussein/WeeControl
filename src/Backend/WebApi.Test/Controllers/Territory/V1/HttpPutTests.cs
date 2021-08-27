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
    public class HttpPutTests :
        IClassFixture<WebApplicationFactory<Startup>>,
        IDisposable
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
                RequestUri = httpMessage.GetUri(ApiRouteEnum.Territory)
            };
        }

        public void Dispose()
        {
            request = null;
        }

        [Fact]
        public async void WhenWithoutToken_ReturnUnAuthorized()
        {
            var uri = new Uri(request.RequestUri, Guid.NewGuid().ToString());
            request.RequestUri = uri;

            var response = await httpMessage.GetResponseMessageAsync(request);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async void WhenRandomTerritory_ReturnNotFound()
        {
            var uri = new Uri(request.RequestUri, Guid.NewGuid().ToString());
            request.RequestUri = uri;

            var dto = new RequestDto<TerritoryDto>()
            {
                DeviceId = httpMessage.DeviceId,
                Payload = new TerritoryDto()
                {
                    CountryId = "PUT", Name = new Random().NextDouble().ToString()
                }
            };

            request.Content = httpMessage.GetHttpContentAsJson(dto);

            var token = await userToken.GetTokenAsync("admin", "admin");

            var response = await httpMessage.GetResponseMessageAsync(request, token);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
