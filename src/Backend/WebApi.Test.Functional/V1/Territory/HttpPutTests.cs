using System;
using System.Net;
using System.Net.Http;
using WeeControl.SharedKernel.Common;
using WeeControl.SharedKernel.DtosV1;
using WeeControl.SharedKernel.DtosV1.Territory;
using Xunit;

namespace WeeControl.Backend.WebApi.Test.Functional.V1.Territory
{
    public class HttpPutTests :
        BaseFunctionalTest,
        IClassFixture<CustomWebApplicationFactory<Startup>>,
        IDisposable
    {
        public HttpPutTests(CustomWebApplicationFactory<Startup> factory) :
            base(factory, HttpMethod.Put, typeof(HttpGetTests).Namespace)
        {
            ServerUri = GetUri(ApiRouteEnum.Territory);
        }

        [Fact]
        public async void WhenWithoutToken_ReturnUnAuthorized()
        {
            var uri = new Uri(ServerUri, Guid.Empty.ToString());

            var response = await GetResponseMessageAsync(uri, GetHttpContentAsJson(new TerritoryDto()));

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async void WhenRandomTerritory_ReturnNotFound()
        {
            var uri = new Uri(ServerUri, Guid.Empty.ToString());

            var dto = new RequestDto<TerritoryDto>()
            {
                DeviceId = DeviceId,
                Payload = new TerritoryDto()
                {
                    CountryId = "PUT",
                    Name = new Random().NextDouble().ToString()
                }
            };

            var token = await authorization.GetTokenAsync("admin", "admin");

            var response = await GetResponseMessageAsync(uri, GetHttpContentAsJson(dto), token);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
