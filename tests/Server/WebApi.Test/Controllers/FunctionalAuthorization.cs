using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WeeControl.SharedKernel.Aggregates.Employee.DtosV1;
using WeeControl.SharedKernel.Common;

namespace WeeControl.Server.WebApi.Test.Controllers
{
    public class FunctionalAuthorization
    {
        private readonly HttpClient client;
        private readonly Uri uri;

        public FunctionalAuthorization(HttpClient client)
        {
            this.client = client;
            uri = new Uri(new Uri(new CommonLists().GetRoute(ApiRouteEnum.Base)), new CommonLists().GetRoute(ApiRouteEnum.EmployeeSession)); ;
        }

        public async Task<string> GetTokenAsync(string username, string password, string device)
        {
            var loginRequestDto = new CreateLoginDto()
            {
                Username = username,
                Password = password,
            };

            var loginToken = await CreateTokenAsync(loginRequestDto);
            return await RefreshTokenAsync(loginToken);
        }

        private async Task<string> CreateTokenAsync(CreateLoginDto loginDto)
        {
            var response = await client.PostAsJsonAsync(uri, loginDto);
            response.EnsureSuccessStatusCode();
            var tokenDto = await response.Content.ReadFromJsonAsync<EmployeeTokenDto>();

            var token = tokenDto.Token;

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token);

            return token;
        }

        private async Task<string> RefreshTokenAsync(string token)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token);

            var dto2 = new RefreshLoginDto();
            //
            var response2 = await client.PutAsJsonAsync(uri, dto2);
            response2.EnsureSuccessStatusCode();
            var tokenDto2 = await response2.Content.ReadFromJsonAsync<EmployeeTokenDto>();
            var token2 = tokenDto2.Token;
            //
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token2);

            return token2;
        }
    }
}
