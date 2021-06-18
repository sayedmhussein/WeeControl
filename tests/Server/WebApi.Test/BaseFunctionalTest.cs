using System;
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
    public enum EmployeeName { Admin, User };

    public class BaseFunctionalTest
    {
        private readonly HttpClient client;
        internal string Token { get; }
        internal string Device { get; private set; }

        internal BaseFunctionalTest(HttpClient client, EmployeeName name)
        {
            Device = typeof(BaseFunctionalTest).Namespace;
            Token = GetTokenAsync(client, name).GetAwaiter().GetResult();
            this.client = client;
        }

        internal HttpContent GetHttpContentAsJson(IDto dto)
        {
            string content = JsonConvert.SerializeObject(dto);
            return new StringContent(content, Encoding.UTF8, "application/json");
        }

        internal HttpResponseMessage GetResponseMessage(HttpRequestMessage requestMessage, string token = null)
        {
            if (string.IsNullOrEmpty(token) == false)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            }

            return client.SendAsync(requestMessage).GetAwaiter().GetResult();
        }

        private Task<string> GetTokenAsync(HttpClient client, EmployeeName name)
        {
            return name switch
            {
                EmployeeName.Admin => GetTokenAsync(client, "admin", "admin"),
                EmployeeName.User => GetTokenAsync(client, "user", "user"),
                _ => Task.FromResult(string.Empty),
            };
        }

        private async Task<string> GetTokenAsync(HttpClient client, string username, string password)
        {
            var route = new ApiDicts().ApiRoute[ApiRouteEnum.Employee] + "Session/";
            var metadata = new RequestMetadata() { Device = Device };

            var dto1 = new CreateLoginDto() { Username = username, Password = password, Metadata = metadata };
            //
            var response1 = await client.PostAsJsonAsync(route, dto1);
            response1.EnsureSuccessStatusCode();
            var tokenDto1 = await response1.Content.ReadFromJsonAsync<EmployeeTokenDto>();
            var token1 = tokenDto1.Token;
            //
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token1);

            var dto2 = new RefreshLoginDto() { Metadata = metadata };
            //
            var response2 = await client.PutAsJsonAsync(route, dto2);
            response2.EnsureSuccessStatusCode();
            var tokenDto2 = await response2.Content.ReadFromJsonAsync<EmployeeTokenDto>();
            var token2 = tokenDto2.Token;
            //
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token2);

            return token2;
        }
    }
}
