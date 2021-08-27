using System;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using WeeControl.SharedKernel.Common;
using Xunit;

namespace WeeControl.Backend.WebApi.Test.Controllers
{
    public class FunctionalTestTemplate :
        IClassFixture<WebApplicationFactory<Startup>>,
        IDisposable
    {
        private readonly IHttpMessage httpMessage;
        private readonly IFunctionalAuthorization userToken;
        private HttpRequestMessage request;

        public FunctionalTestTemplate(WebApplicationFactory<Startup> factory)
        {
            var client = factory.CreateClient();

            httpMessage = new HttpMessage(client, typeof(FunctionalTestTemplate).Namespace);
            userToken = new FunctionalAuthorization(httpMessage);

            request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
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
            var response = await httpMessage.GetResponseMessageAsync(request);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Theory]
        [InlineData("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzc3MiOiI3Y2ViZGRlYi1iOGI0LTRhOWUtOTg5Mi05ZjU3ZTA3Yjc0MTIiLCJuYmYiOjE2Mjk5MDM0MjEsImV4cCI6MTYyOTkwMzcyMSwiaWF0IjoxNjI5OTAzNDIxfQ.UovMTJJVuT7DBQJolZnClbNT86qfoeYBt7hhTj1RUuk")]
        [InlineData("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzc3MiOiI3Y2ViZGRlYi1iOGI0LTRhOWUtOTg5Mi05ZjU3ZTA3Yjc0MTIiLCJociI6ImFkZDtlZHQ7ZGVsO3JlZDtyZXY7c25yIiwibmJmIjoxNTI5OTAzNDg0LCJleHAiOjE1MzAzMzU0ODQsImlhdCI6MTUyOTkwMzQ4NH0.I7MjTQpC2MRafL5qQ9p2NEAbR8VnOii_77g1KefssA0")]
        [InlineData("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ8.eyJzc3MiOiI3Y2ViZGRlYi1iOGI0LTRhOWUtOTg5Mi05ZjU3ZTA3Yjc0MTIiLCJociI6ImFkZDtlZHQ7ZGVsO3JlZDtyZXY7c25yIiwibmJmIjoxNjI5OTAzNDg0LCJleHAiOjE2MzAzMzU0ODQsImlhdCI6MTYyOTkwMzQ4NH0.Q0nV7hhdj5sI8nSmatGvIWohzixMtRBUDKklnK9as7g")]
        [InlineData("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzc3MiOiI3Y2ViZGRlYi1iOGI0LTRhOWUtOTg5Mi05ZjU3ZTA3Yjc0MTIiLCJociI6ImFkZDtlZHQ7ZGVsO3JlZDtyZXY7c25yIiwibmJmIjoxNjI5OTAzNDg0LCJleHAiOjE2MzAzMzU0ODQsImlhdCI6MTYyOTkwMzQ4NH9.Q0nV7hhdj5sI8nSmatGvIWohzixMtRBUDKklnK9as7g")]
        [InlineData("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzc3MiOiI3Y2ViZGRlYi1iOGI0LTRhOWUtOTg5Mi05ZjU3ZTA3Yjc0MTIiLCJociI6ImFkZDtlZHQ7ZGVsO3JlZDtyZXY7c25yIiwibmJmIjoxNjI5OTAzNDg0LCJleHAiOjE2MzAzMzU0ODQsImlhdCI6MTYyOTkwMzQ4NH0.Q0nV7hhdj5sI8nSmatGvIWohzixMtRBUDKklnK9as7f")]
        public async void WhenWithExpiredToken_ReturnUnauthorized(string token)
        {
            var response = await httpMessage.GetResponseMessageAsync(request, token);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
