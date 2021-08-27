using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using WeeControl.Backend.Domain.BasicDbos.Territory;
using WeeControl.Backend.Persistence;
using WeeControl.SharedKernel.Aggregates.Territory.DtosV1;
using WeeControl.SharedKernel.Common;
using WeeControl.SharedKernel.DtosV1;
using Xunit;

namespace WeeControl.Backend.WebApi.Test.Functional.V1.Territory
{
    public class HttpGetTests :
        BaseFunctionalTest,
        IClassFixture<CustomWebApplicationFactory<Startup>>,
        IDisposable
    {
        public HttpGetTests(CustomWebApplicationFactory<Startup> factory) :
            base(factory, HttpMethod.Get, typeof(HttpGetTests).Namespace)
        {
        }

        [Fact]
        public async void WhenWithoutToken_ReturnUnauthorized()
        {
            var response = await GetResponseMessageAsync(GetUri(ApiRouteEnum.Territory));

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async void WhenWithToken_ReturnOkWithListOfTypeTerritory()
        {
            using var serviceScope = factory.Services.CreateScope();
            var context = serviceScope.ServiceProvider.GetService<MySystemDbContext>();
            await context.Territories.AddAsync(new TerritoryDbo() { CountryId = "001", Name = "001" });
            await context.Territories.AddAsync(new TerritoryDbo() { CountryId = "002", Name = "002" });
            await context.SaveChangesAsync();

            var token = await authorization.GetTokenAsync("admin", "admin");

            var response = await GetResponseMessageAsync(GetUri(ApiRouteEnum.Territory), null, token);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var response_str = await response.Content.ReadAsStringAsync();
            var response_dto = JsonConvert.DeserializeObject<ResponseDto<IEnumerable<IdentifiedTerritoryDto>>>(response_str);

            var territories = response_dto.Payload;
            Assert.NotEmpty(territories);
            Assert.Equal(3, territories.Count());
        }

        [Fact]
        public async void WhenQueringSpecificTerritory_ReturnCorrectCount()
        {
            using var serviceScope = factory.Services.CreateScope();
            var context = serviceScope.ServiceProvider.GetService<MySystemDbContext>();
            var parent = context.Territories.FirstOrDefault();
            await context.Territories.AddAsync(new TerritoryDbo() { CountryId = "001", Name = "001", ReportToId = parent.Id });
            await context.Territories.AddAsync(new TerritoryDbo() { CountryId = "002", Name = "002" });
            await context.SaveChangesAsync();

            var token = await authorization.GetTokenAsync("admin", "admin");
            var uri = new UriBuilder(GetUri(ApiRouteEnum.Territory));
            uri.Query = parent.Id.ToString();
            var response = await GetResponseMessageAsync(uri.Uri, null, token);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var response_str = await response.Content.ReadAsStringAsync();
            var response_dto = JsonConvert.DeserializeObject<ResponseDto<IEnumerable<IdentifiedTerritoryDto>>>(response_str);

            var territories = response_dto.Payload;
            Assert.NotEmpty(territories);
            Assert.Equal(2, territories.Count());
        }
    }
}
