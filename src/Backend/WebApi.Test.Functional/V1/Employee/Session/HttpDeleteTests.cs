using System;
using System.Net;
using System.Net.Http;
using WeeControl.Backend.WebApi.Test.Functional.TestHelpers;
using WeeControl.SharedKernel.Routing;
using Xunit;

namespace WeeControl.Backend.WebApi.Test.Functional.V1.Employee.Session
{
    [CollectionDefinition("Non-Parallel Collection", DisableParallelization = true)]
    public class HttpDeleteTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> factory;
        private readonly IFunctionalTest test;
        private readonly IFunctionalAuthorization authorization;
        private readonly Uri routeUri;
        
        public HttpDeleteTests(CustomWebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
            test = new FunctionalTest(factory, HttpMethod.Delete, typeof(HttpDeleteTests).Namespace);
            authorization = new FunctionalAuthorization(test);
            routeUri = test.GetUri(ApiRouteEnum.EmployeeSession);
        }

        [Fact]
        public async void WhenTerminatingWithRandomToken_ReturnUnauthorized()
        {
            var response = await test.GetResponseMessageAsync(routeUri, null, "khkjhhkhk");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async void WhenTerminatingExistingSession_ReturnOK()
        {
            var token = await authorization.GetTokenAsync("admin", "admin");

            var response = await test.GetResponseMessageAsync(routeUri, null, token);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
