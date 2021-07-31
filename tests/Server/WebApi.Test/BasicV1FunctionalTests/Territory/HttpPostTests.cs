using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using WeeControl.SharedKernel.Aggregates.Employee.Entities.DtosV1;
using WeeControl.SharedKernel.Aggregates.Territory.Entities.DtosV1;
using Xunit;

namespace WeeControl.Server.WebApi.Test.BasicV1FunctionalTests.Territory
{
    public class HttpPostTests :
        BaseFunctionalTest,
        IClassFixture<WebApplicationFactory<Startup>>,
        IDisposable
    {
        HttpRequestMessage request;
        TerritoryDto territoryDto;

        public HttpPostTests(WebApplicationFactory<Startup> factory) :
            base(factory.CreateClient())
        {
            request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                Version = new Version("1.0"),
                RequestUri = GetUri(SharedKernel.Common.Enums.ApiRouteEnum.Territory)
            };

            territoryDto = new TerritoryDto()
            {
                CountryId = "UAE",
                Name = new Random().NextDouble().ToString(),
            };
        }

        public void Dispose()
        {
            request = null;
            territoryDto = null;
            Token = null;
        }

        [Fact]
        public async void WhenAddingTerritoryWithoutToken_ReturnUnauthorizedHttpCodeRespose()
        {
            request.Content = GetHttpContentAsJson(territoryDto);
            var response = await GetResponseMessageAsync(request);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async void WhenAddingCorrectTerritoryWithToken_ReturnCreatedHttpCodeRespose()
        {
            await AuthorizeAsync("admin", "admin");

            request.Content = GetHttpContentAsJson(territoryDto);
            var response = await GetResponseMessageAsync(request);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async void WhenAddingBadTerritory_ReturnBadRequestHttpCodeRespose()
        {
            await AuthorizeAsync("admin", "admin");

            request.Content = GetHttpContentAsJson(new EmployeeDto());
            var response = await GetResponseMessageAsync(request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async void WhenAddingCorrectTerritoryButRandomParent_ReturnConflictHttpCodeRespose()
        {
            await AuthorizeAsync("admin", "admin");
            territoryDto.ReportToId = Guid.Empty;

            request.Content = GetHttpContentAsJson(territoryDto);
            var response = await GetResponseMessageAsync(request);

            Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        }

        [Fact]
        public async void WhenAddingCorrectTerritoryAndCorrectParentWithToken_ReturnCreatedHttpCodeRespose()
        {
            await AuthorizeAsync("admin", "admin");

            var request1 = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                Version = new Version("1.0"),
                RequestUri = GetUri(SharedKernel.Common.Enums.ApiRouteEnum.Territory)
            };

            var response1 = await GetResponseMessageAsync(request1);
            response1.EnsureSuccessStatusCode();
            var list = await response1.Content.ReadFromJsonAsync<IEnumerable<TerritoryDto>>();

            territoryDto.ReportToId = list.FirstOrDefault().Id;

            request.Content = GetHttpContentAsJson(territoryDto);
            var response = await GetResponseMessageAsync(request);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }
    }
}
