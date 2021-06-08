using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using MySystem.SharedKernel.EntityV1Dtos.Employee;
using MySystem.SharedKernel.Enumerators;
using MySystem.SharedKernel.Services;
using Xunit;

namespace MySystem.Web.Api.Test.FunctionalTests
{
    public class Common : IClassFixture<WebApplicationFactory<Startup>>
    {
        internal async static Task<string> GetNewAdminTokenAsync(HttpClient client)
        {
            var route = new SharedValues().ApiRoute[ApiRouteEnum.Employee] + "Session/";

            var dto1 = new LoginDto() { Username = "admin", Password = "admin", Device = "device" };
            //
            var response1 = await client.PostAsJsonAsync(route, dto1);
            response1.EnsureSuccessStatusCode();
            var tokenDto1 = await response1.Content.ReadFromJsonAsync<EmployeeTokenDto>();
            var token1 = tokenDto1.Token;
            //
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token1);

            var dto2 = new LoginRefreshDto() { Device = "device" };
            //
            var response2 = await client.PutAsJsonAsync(route, dto2);
            response2.EnsureSuccessStatusCode();
            var tokenDto2 = await response2.Content.ReadFromJsonAsync<EmployeeTokenDto>();
            var token2 = tokenDto2.Token;
            //
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token2);

            return token2;
        }

        private readonly WebApplicationFactory<Startup> factory;

        public Common(WebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
        }

        [Fact]
        public async void WhenRefreshLoginOfAdmin_ReturnTokenString()
        {
            var client = factory.CreateClient();

            var token = await GetNewAdminTokenAsync(client);

            Assert.NotEmpty(token);
        }
    }
}
