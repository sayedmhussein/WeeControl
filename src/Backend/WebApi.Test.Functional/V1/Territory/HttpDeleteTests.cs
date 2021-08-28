using System;
using System.Net;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using WeeControl.Backend.Domain.BasicDbos.Territory;
using WeeControl.Backend.Persistence;
using WeeControl.SharedKernel.Common;
using WeeControl.SharedKernel.DtosV1;
using Xunit;

namespace WeeControl.Backend.WebApi.Test.Functional.V1.Territory
{
    public class HttpDeleteTests :
        BaseFunctionalTest,
        IClassFixture<CustomWebApplicationFactory<Startup>>,
        IDisposable
    {
        public HttpDeleteTests(CustomWebApplicationFactory<Startup> factory) :
            base(factory, HttpMethod.Delete, typeof(HttpGetTests).Namespace)
        {
            ServerUri = GetUri(ApiRouteEnum.Territory);
        }

        [Fact]
        public async void WhenWithoutToken_ReturnUnauthorized()
        {
            var uri = new Uri(ServerUri, Guid.Empty.ToString());
            var dto = new RequestDto<object>() { DeviceId = DeviceId };
            var response = await GetResponseMessageAsync(uri, GetHttpContentAsJson(dto));

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

            var uri = new Uri(ServerUri, territory.Id.ToString());
            var dto = new RequestDto<object>() { DeviceId = DeviceId };
            var token = await authorization.GetTokenAsync("admin", "admin");
            var response = await GetResponseMessageAsync(uri, GetHttpContentAsJson(dto), token);

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async void WhenNotExisting_ReturnNoContent()
        {
            var territory = new TerritoryDbo() { CountryId = "001", Name = "001" };

            var uri = new Uri(ServerUri, Guid.NewGuid().ToString());
            var dto = new RequestDto<object>() { DeviceId = DeviceId };
            var token = await authorization.GetTokenAsync("admin", "admin");
            var response = await GetResponseMessageAsync(uri, GetHttpContentAsJson(dto), token);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
