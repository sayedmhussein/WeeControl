using System;
using System.Collections.Immutable;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WeeControl.SharedKernel.BasicSchemas.Common.Dicts;
using WeeControl.SharedKernel.BasicSchemas.Common.DtosV1;
using WeeControl.SharedKernel.BasicSchemas.Common.Enums;
using WeeControl.SharedKernel.BasicSchemas.Common.Interfaces;
using WeeControl.SharedKernel.BasicSchemas.Employee.DtosV1;

namespace WeeControl.Server.WebApi.Test
{
    public class BaseFunctionalTest
    {
        private readonly HttpClient client;
        private readonly string sessionRoute;


        internal ImmutableDictionary<ApiRouteEnum, string> ApiRoute { get; private set; }

        internal string Token { get; set; }
        internal RequestMetadataV1 Metadata { get; set; }

        internal BaseFunctionalTest(HttpClient client)
        {
            this.client = client;

            var apiRoutes = new ApiDicts();
            ApiRoute = apiRoutes.ApiRoute;

            sessionRoute = apiRoutes.ApiRoute[ApiRouteEnum.Employee] + "Session/";

            Token = string.Empty;
            Metadata = new RequestMetadataV1() { Device = typeof(BaseFunctionalTest).Namespace };
        }

        internal HttpContent GetHttpContentAsJson(IDto dto)
        {
            string content = JsonConvert.SerializeObject(dto);
            return new StringContent(content, Encoding.UTF8, "application/json");
        }

        internal Task<HttpResponseMessage> GetResponseMessageAsync(HttpRequestMessage requestMessage)
        {
            if (string.IsNullOrEmpty(Token) == false)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Token);
            }

            return client.SendAsync(requestMessage);
        }

        internal async Task CreateTokenAsync(CreateLoginDto loginDto)
        {
            var response = await client.PostAsJsonAsync(sessionRoute, loginDto);
            response.EnsureSuccessStatusCode();
            var tokenDto = await response.Content.ReadFromJsonAsync<EmployeeTokenDto>();

            Token = tokenDto.Token;
            Metadata = loginDto.Metadata;

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Token);
        }

        internal async Task RefreshTokenAsync()
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Token);

            var dto2 = new RefreshLoginDto() { Metadata = Metadata };
            //
            var response2 = await client.PutAsJsonAsync(sessionRoute, dto2);
            response2.EnsureSuccessStatusCode();
            var tokenDto2 = await response2.Content.ReadFromJsonAsync<EmployeeTokenDto>();
            var token2 = tokenDto2.Token;
            //
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token2);

            Token = token2;
        }
    }
}
