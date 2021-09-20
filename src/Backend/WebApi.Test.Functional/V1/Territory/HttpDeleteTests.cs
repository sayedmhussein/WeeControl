using System;
using System.Net;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using WeeControl.Backend.Domain.EntityGroups.Territory;
using WeeControl.Backend.Persistence;
using WeeControl.Backend.WebApi.Test.Functional.TestHelpers;
using WeeControl.Common.SharedKernel.DtosV1;
using WeeControl.Common.SharedKernel.Routing;
using Xunit;

namespace WeeControl.Backend.WebApi.Test.Functional.V1.Territory
{
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
            routeUri = test.GetUri(ApiRouteEnum.Territory);
        }

        [Fact]
        public async void WhenWithoutToken_ReturnUnauthorized()
        {
            var uri = new Uri(routeUri, Guid.Empty.ToString());
            var dto = new RequestDto<object>() { DeviceId = test.DeviceId };
            var response = await test.GetResponseMessageAsync(uri, test.GetHttpContentAsJson(dto));

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async void WhenExisting_ReturnNoContent()
        {
            var territory = new TerritoryDbo() { CountryId = "001", Name = "001" };
            using var serviceScope = factory.Services.CreateScope();
            var context = serviceScope.ServiceProvider.GetService<MySystemDbContext>();
            await context.Territories.AddAsync(territory);
            await context.SaveChangesAsync();

            var uri = new Uri(routeUri, territory.Id.ToString());
            var dto = new RequestDto<object>() { DeviceId = test.DeviceId };
            var token = await authorization.GetTokenAsync("admin", "admin");
            var response = await test.GetResponseMessageAsync(uri, test.GetHttpContentAsJson(dto), token);

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async void WhenNotExisting_ReturnNoContent()
        {
            var territory = new TerritoryDbo() { CountryId = "001", Name = "001" };

            var uri = new Uri(routeUri, Guid.NewGuid().ToString());
            var dto = new RequestDto<object>() { DeviceId = test.DeviceId };
            var token = await authorization.GetTokenAsync("admin", "admin");
            var response = await test.GetResponseMessageAsync(uri, test.GetHttpContentAsJson(dto), token);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
