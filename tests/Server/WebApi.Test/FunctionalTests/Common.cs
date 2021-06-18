using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using WeeControl.SharedKernel.CommonSchemas.Common.Dicts;
using WeeControl.SharedKernel.CommonSchemas.Common.DtosV1;
using WeeControl.SharedKernel.CommonSchemas.Common.Enums;
using WeeControl.SharedKernel.CommonSchemas.Employee.DtosV1;
using Xunit;

namespace WeeControl.Server.WebApi.Test.FunctionalTests
{
    public class Common : IClassFixture<WebApplicationFactory<Startup>>
    {
        internal async static Task<string> GetNewAdminTokenAsync(HttpClient client)
        {
            var route = new ApiDicts().ApiRoute[ApiRouteEnum.Employee] + "Session/";
            var metadata = new RequestMetadata() { Device = "device" };

            var dto1 = new CreateLoginDto() { Username = "admin", Password = "admin", Metadata = metadata };
            //
            var response1 = await client.PostAsJsonAsync(route, dto1);
            response1.EnsureSuccessStatusCode();
            var tokenDto1 = await response1.Content.ReadFromJsonAsync<EmployeeTokenDto>();
            var token1 = tokenDto1.Token;
            //
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token1);

            var dto2 = new RefreshLoginDto() { Metadata = metadata};
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
