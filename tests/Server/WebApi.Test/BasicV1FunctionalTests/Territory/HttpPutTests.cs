using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using WeeControl.SharedKernel.Aggregates.Territory.Entities.DtosV1;
using Xunit;

namespace WeeControl.Server.WebApi.Test.BasicV1FunctionalTests.Territory
{
    public class HttpPutTests :
        BaseFunctionalTest,
        IClassFixture<WebApplicationFactory<Startup>>,
        IDisposable
    {
        HttpRequestMessage request;

        public HttpPutTests(WebApplicationFactory<Startup> factory) :
            base(factory.CreateClient())
        {
            request = new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                Version = new Version("1.0"),
                RequestUri = GetUri(SharedKernel.Common.Enums.ApiRouteEnum.Territory)
            };
        }

        public void Dispose()
        {
            request = null;
            Token = null;
        }

        [Fact]
        public void Example()
        {
            Assert.NotNull(RequestMetadata);
        }

        [Fact]
        public async void WhenUpdatingTerritoryWithoutToken_ReturnUnauthorizedHttpCodeRespose()
        {
            var response = await GetResponseMessageAsync(request);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async void WhenUpdatingBadTerritory_ReturnBadRequestHttpCodeRespose()
        {
            await AuthorizeAsync("admin", "admin");

            request.Content = GetHttpContentAsJson(new TerritoryDto());

            var response = await GetResponseMessageAsync(request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async void WhenUpdatingExisintTerritoryWithToken_ReturnNoContentHttpCodeRespose()
        {
            await AuthorizeAsync("admin", "admin");

            var request1 = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                Version = new Version("1.0"),
                RequestUri = GetUri(SharedKernel.Common.Enums.ApiRouteEnum.Territory)
            };
            var response1 = await GetResponseMessageAsync(request1);
            response1.EnsureSuccessStatusCode();
            var list = await response1.Content.ReadFromJsonAsync<IEnumerable<TerritoryDto>>();

            request.Content = GetHttpContentAsJson(new TerritoryDto() { Id = list.FirstOrDefault().Id, CountryId = "EGP", Name = new Random().NextDouble().ToString() });

            var response = await GetResponseMessageAsync(request);

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async void WhenUpdatingNotExisintTerritoryWithToken_ReturnNotFoundHttpCodeRespose()
        {
            await AuthorizeAsync("admin", "admin");

            request.Content = GetHttpContentAsJson(new TerritoryDto() { Id = Guid.Empty, CountryId = "EGP", Name = new Random().NextDouble().ToString() });

            var response = await GetResponseMessageAsync(request);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
