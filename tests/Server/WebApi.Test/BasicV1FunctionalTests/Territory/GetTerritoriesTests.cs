using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Web;
using Microsoft.AspNetCore.Mvc.Testing;
using WeeControl.SharedKernel.BasicSchemas.Common.Enums;
using WeeControl.SharedKernel.BasicSchemas.Territory.DtosV1;
using Xunit;

namespace WeeControl.Server.WebApi.Test.BasicV1FunctionalTests.Territory
{
    public class GetTerritoriesTests : BaseFunctionalTest, IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly Uri RequstUri;
        private readonly WebApplicationFactory<Startup> factory;

        public GetTerritoriesTests(WebApplicationFactory<Startup> factory)
            : base(factory.CreateClient(), EmployeeName.Admin)
        {
            RequstUri = new Uri(BaseUri, ApiRoute[ApiRouteEnum.Territory]);
            this.factory = factory;
        }

        [Fact]
        public async void WhenGettingAllTerritoriesWithoutToken_ResponseIsUnauthorized()
        {
            var client = factory.CreateClient();

            var response = await client.GetAsync(RequstUri);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async void WhenGettingAllTerritoriesWithTokenButWithoutQuery_ResponseIsUnauthorized()
        {
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Token);

            var response = await client.GetAsync(RequstUri);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async void WhenGettingAllTerritoriesWithTokenAndWithRandomTerritoryIdQuery_ResponseOk()
        {
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Token);

            var builder = new UriBuilder(RequstUri);
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["territoryid"] = Guid.NewGuid().ToString();
            builder.Query = query.ToString();

            var response = await client.GetAsync(builder.ToString());

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact(Skip = "Territory Is is hand written which may be differet from acutal in database")]
        public async void WhenGettingAllTerritoriesWithTokenAndWithKnownTerritoryIdQuery_ResponseOkAndListOfTerritores()
        {
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Token);

            var builder = new UriBuilder(RequstUri);
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["territoryid"] = "aa1a5745-b69d-46cf-b3e8-b21e8d76be5a";
            builder.Query = query.ToString();

            var response = await client.GetAsync(builder.ToString());

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var list = await response.Content.ReadFromJsonAsync<IEnumerable<TerritoryDto>>();
            Assert.NotEmpty(list);
        }

        [Fact]
        public async void WhenGettingAllTerritoriesWithTokenAndWithRandomEmployeeIdQuery_ResponseOk()
        {
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Token);

            var builder = new UriBuilder(RequstUri);
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["employeeid"] = Guid.NewGuid().ToString();
            builder.Query = query.ToString();

            var response = await client.GetAsync(builder.ToString());

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
