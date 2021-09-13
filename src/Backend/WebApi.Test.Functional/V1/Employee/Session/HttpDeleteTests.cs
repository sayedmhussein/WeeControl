using System;
using System.Net;
using System.Net.Http;
using WeeControl.SharedKernel.Helpers;
using Xunit;

namespace WeeControl.Backend.WebApi.Test.Functional.V1.Employee.Session
{
    [CollectionDefinition("Non-Parallel Collection", DisableParallelization = true)]
    public class HttpDeleteTests :
        BaseFunctionalTest,
        IClassFixture<CustomWebApplicationFactory<Startup>>,
        IDisposable
    {
        public HttpDeleteTests(CustomWebApplicationFactory<Startup> factory) :
            base(factory, HttpMethod.Delete, typeof(HttpDeleteTests).Namespace)
        {
            ServerUri = GetUri(ApiRouteEnum.EmployeeSession);
        }

        [Fact]
        public async void WhenTerminatingWithRandomToken_ReturnUnauthorized()
        {
            var response = await GetResponseMessageAsync(ServerUri, null, "khkjhhkhk");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async void WhenTerminatingExistingSession_ReturnOK()
        {
            var token = await authorization.GetTokenAsync("admin", "admin");

            var response = await GetResponseMessageAsync(ServerUri, null, token);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
