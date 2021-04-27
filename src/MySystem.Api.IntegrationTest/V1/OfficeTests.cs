using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using MySystem.Api.Dtos.V1;
using Newtonsoft.Json;
using Xunit;

namespace MySystem.Api.FunctionalTest.V1
{
    public class OfficeTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> factory;
        private readonly string Url;

        public OfficeTests(WebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
            Url = "Api/Office";
        }

        [Fact]
        public async Task<int> WhenGettingAllOfficesReturnListOfOffices()
        {
            var client = factory.CreateClient();
            var response = await client.GetAsync(Url);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var bla = await response.Content.ReadAsStringAsync();
            var blabla = JsonConvert.DeserializeObject<ResponseDto<List<OfficeDto>>>(bla);
            return blabla.Payload.Count;
        }

        [Fact]
        public async Task<Guid> WhenPostingNewOfficeReturnSameOfficeWithNewId()
        {
            var client = factory.CreateClient();

            var request = new OfficeDto()
            {
                OfficeName = "This Office was created during testing...",
                CountryId = "EGP",
                ParentId = Guid.Parse("12345678-abcd-1234-abcd-123456789abc")
            };
            var body = JsonConvert.SerializeObject(new RequestDto<OfficeDto>(request) { DeviceId = "bla" });
            var content = new StringContent(body, Encoding.UTF8, "application/json");
            var response = await client.PutAsync(Url, content);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var bla = await response.Content.ReadAsStringAsync();
            var blabla = JsonConvert.DeserializeObject<ResponseDto<OfficeDto>>(bla);
            return (Guid)blabla.Payload.Id;
        }

        [Fact]
        public async Task WhenDeletingOfficeTheTotalCountShouldBeLess()
        {
            var id = await WhenPostingNewOfficeReturnSameOfficeWithNewId();
            var count = await WhenGettingAllOfficesReturnListOfOffices();

            var client = factory.CreateClient();
            var response = await client.DeleteAsync(Url + "/" + id);

            response.EnsureSuccessStatusCode();
            Assert.True(count > await WhenGettingAllOfficesReturnListOfOffices());
        }
    }
}
