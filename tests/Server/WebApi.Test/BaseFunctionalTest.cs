using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WeeControl.SharedKernel.Common;
using WeeControl.SharedKernel.Common.DtosV1;
using WeeControl.SharedKernel.Common.Enums;
using WeeControl.SharedKernel.Common.Interfaces;
using WeeControl.SharedKernel.Aggregates.Employee.Entities.DtosV1;

namespace WeeControl.Server.WebApi.Test
{
    public class BaseFunctionalTest
    {
        internal string Token { get; set; }

        protected ICommonLists CommonLists { get; private set; }
        protected IRequestMetadata RequestMetadata { get; set; }

        private readonly HttpClient client;
        private readonly string sessionRoute;

        internal BaseFunctionalTest(HttpClient client)
        {
            this.client = client;

            CommonLists = new CommonLists();

            sessionRoute = CommonLists.GetRoute(ApiRouteEnum.Employee) + "Session/";

            Token = string.Empty;
            RequestMetadata = new RequestMetadataV1() { Device = typeof(BaseFunctionalTest).Namespace };
        }

        internal async Task AuthorizeAsync(string username, string password)
        {
            var loginRequestDto = new CreateLoginDto()
            {
                Username = username,
                Password = password,
                Metadata = (RequestMetadataV1)RequestMetadata
            };

            await CreateTokenAsync(loginRequestDto);
            await RefreshTokenAsync();
        }

        internal Task<HttpResponseMessage> GetResponseMessageAsync(HttpRequestMessage requestMessage)
        {
            if (string.IsNullOrEmpty(Token) == false)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Token);
            }

            return client.SendAsync(requestMessage);
        }

        protected HttpContent GetHttpContentAsJson(IDto dto)
        {
            string content = JsonConvert.SerializeObject(dto);
            return new StringContent(content, Encoding.UTF8, "application/json");
        }

        protected Uri GetUri(ApiRouteEnum route)
        {
            return new Uri(new Uri(CommonLists.GetRoute(ApiRouteEnum.Base)), CommonLists.GetRoute(route));
        }

        internal async Task CreateTokenAsync(CreateLoginDto loginDto)
        {
            var response = await client.PostAsJsonAsync(sessionRoute, loginDto);
            response.EnsureSuccessStatusCode();
            var tokenDto = await response.Content.ReadFromJsonAsync<EmployeeTokenDto>();

            Token = tokenDto.Token;
            RequestMetadata = loginDto.Metadata;

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Token);
        }

        internal async Task RefreshTokenAsync()
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Token);

            var dto2 = new RefreshLoginDto() { Metadata = (RequestMetadataV1)RequestMetadata };
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
