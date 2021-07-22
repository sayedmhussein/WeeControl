using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Web;
using Microsoft.AspNetCore.Mvc.Testing;
using WeeControl.SharedKernel.BasicSchemas.Territory.Entities.DtosV1;
using Xunit;

namespace WeeControl.Server.WebApi.Test.BasicV1FunctionalTests.Territory
{
    public class HttpGetTests :
        BaseFunctionalTest,
        IClassFixture<WebApplicationFactory<Startup>>, IDisposable
    {
        HttpRequestMessage request;

        public HttpGetTests(WebApplicationFactory<Startup> factory) :
            base(factory.CreateClient())
        {
            request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                Version = new Version("1.0"),
                RequestUri = GetUri(SharedKernel.BasicSchemas.Common.Enums.ApiRouteEnum.Territory)
            };
        }

        public void Dispose()
        {
            request = null;
            Token = null;
        }

        [Fact]
        public async void WhenUnAuthorizedRequest_ResponseIsUnAuthorized()
        {
            var response = await GetResponseMessageAsync(request);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async void WhenAuthorizedRequest_ResponseIsOk()
        {
            await AuthorizeAsync("admin", "admin");

            var response = await GetResponseMessageAsync(request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async void WhenAuthorizedRequest_ResponseHasListOfTerritories()
        {
            await AuthorizeAsync("admin", "admin");

            var response = await GetResponseMessageAsync(request);
            response.EnsureSuccessStatusCode();
            var list = await response.Content.ReadFromJsonAsync<IEnumerable<TerritoryDto>>();

            Assert.NotEmpty(list);
        }

        [Fact]
        public async void WhenAuthorizedRequestAndWithKnownTerritoryIdQuery_ReturnListOfTerritores()
        {
            await AuthorizeAsync("admin", "admin");

            var response1 = await GetResponseMessageAsync(request);
            response1.EnsureSuccessStatusCode();
            var list1 = await response1.Content.ReadFromJsonAsync<IEnumerable<TerritoryDto>>();

            var builder = new UriBuilder(request.RequestUri);
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["id"] = list1.FirstOrDefault().Id.ToString();
            builder.Query = query.ToString();

            request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                Version = new Version("1.0"),
                RequestUri = builder.Uri
            };

            var response2 = await GetResponseMessageAsync(request);
            response2.EnsureSuccessStatusCode();
            var list2 = await response2.Content.ReadFromJsonAsync<IEnumerable<TerritoryDto>>();

            Assert.NotEmpty(list2);
        }

        [Fact]
        public async void WhenGettingAllTerritoriesWithTokenAndWithEmptyGuidInTerritoryQuery_ResponseIsNotFound()
        {
            await AuthorizeAsync("admin", "admin");

            var builder = new UriBuilder(request.RequestUri);
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["id"] = Guid.Empty.ToString();
            builder.Query = query.ToString();
            request.RequestUri = builder.Uri;

            var response = await GetResponseMessageAsync(request);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
