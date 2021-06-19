using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using WeeControl.SharedKernel.BasicSchemas.Common.Enums;
using WeeControl.SharedKernel.BasicSchemas.Territory.DtosV1;
using Xunit;

namespace WeeControl.Server.WebApi.Test.BasicV1FunctionalTests.Territory
{
    public class UpdateTerritoriesTests : BaseFunctionalTest, IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly Uri RequstUri;
        private readonly WebApplicationFactory<Startup> factory;

        public UpdateTerritoriesTests(WebApplicationFactory<Startup> factory)
            : base(factory.CreateClient(), EmployeeName.Admin)
        {
            RequstUri = new Uri(BaseUri, ApiRoute[ApiRouteEnum.Territory]);
            this.factory = factory;
        }

        [Fact]
        public async void WhenUpdatingTerritoryWithoutToken_ReturnUnauthorizedHttpCodeRespose()
        {
            var client = factory.CreateClient();
            var content = GetHttpContentAsJson(new TerritoryDto());

            var response = await client.PutAsync(RequstUri, content);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async void WhenUpdatingBadTerritory_ReturnBadRequestHttpCodeRespose()
        {
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Token);
            var content = GetHttpContentAsJson(new TerritoryDto());

            var response = await client.PutAsync(RequstUri, content);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async void WhenUpdatingExisintTerritoryWithToken_ReturnNoContentHttpCodeRespose()
        {
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Token);

            var response1 = await client.GetAsync(RequstUri);
            response1.EnsureSuccessStatusCode();
            var list = await response1.Content.ReadFromJsonAsync<IEnumerable<TerritoryDto>>();

            var content = GetHttpContentAsJson(new TerritoryDto() { Id = list.FirstOrDefault().Id, CountryId = "EGP", Name = new Random().NextDouble().ToString() });

            var response = await client.PutAsync(RequstUri, content);

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async void WhenUpdatingNotExisintTerritoryWithToken_ReturnNotFoundHttpCodeRespose()
        {
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Token);
            var content = GetHttpContentAsJson(new TerritoryDto() { Id = Guid.Empty, CountryId = "EGP", Name = new Random().NextDouble().ToString() });

            var response = await client.PutAsync(RequstUri, content);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
