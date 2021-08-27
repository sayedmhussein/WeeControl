using System;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using WeeControl.SharedKernel.Common;
using Xunit;

namespace WeeControl.Backend.WebApi.Test.Controllers.Employee.V1.Session
{
    public class HttpDeleteTests : IClassFixture<WebApplicationFactory<Startup>>, IDisposable
    {
        private readonly IHttpMessage httpMessage;
        private readonly IFunctionalAuthorization userToken;
        private HttpRequestMessage request;
    
        public HttpDeleteTests(WebApplicationFactory<Startup> factory)
        {
            var client = factory.CreateClient();

            httpMessage = new HttpMessage(client, typeof(FunctionalTestTemplate).Namespace);
            userToken = new FunctionalAuthorization(httpMessage);

            request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                Version = new Version("1.0"),
                RequestUri = httpMessage.GetUri(ApiRouteEnum.EmployeeSession)
            };
        }

        public void Dispose()
        {
            request = null;
        }

        [Fact]
        public async void WhenTerminatingWithRandomToken_ReturnUnauthorized()
        {
            var response = await httpMessage.GetResponseMessageAsync(request);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async void WhenTerminatingExistingSession_ReturnOK()
        {
            var token = await userToken.GetTokenAsync("admin", "admin");

            var response = await httpMessage.GetResponseMessageAsync(request, token);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
