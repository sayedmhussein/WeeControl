using System;
using System.Net;
using System.Net.Http;
using System.Web;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace WeeControl.Server.WebApi.Test.BasicV1FunctionalTests.Territory
{
    public class HttpDeleteTests :
        BaseFunctionalTest,
        IClassFixture<WebApplicationFactory<Startup>>,
        IDisposable
    {
        HttpRequestMessage request;

        public HttpDeleteTests(WebApplicationFactory<Startup> factory) :
            base(factory.CreateClient())
        {
            request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
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
        public async void WhenDeletingTerritoryWithoutToken_ReturnUnauthorizedHttpCodeRespose()
        {
            var builder = new UriBuilder(request.RequestUri);
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["id"] = Guid.Empty.ToString();
            builder.Query = query.ToString();

            request.RequestUri = builder.Uri;

            var response = await GetResponseMessageAsync(request);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
