using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using MySystem.SharedKernel.Entities.Public.V1Dto;
using Xunit;

namespace MySystem.Web.Api.Test.Controller.V1.Employee.Credentials
{
    public class TokenTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> factory;

        public TokenTests(WebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
        }

        [Fact]
        public async void WhenValidTokenSent_NewTokenRecevied()
        {
            var client = factory.CreateClient();
            var oldToken = await LoginTests.GetTokenAsValidUserAsync(client);
            var newToken = await RefreshTokenAsync(client, oldToken);

            Assert.NotEmpty(newToken);
            Assert.NotEqual(oldToken, newToken);
        }

        [Fact]
        public async void WhenValidTokenSent2_NewTokenRecevied()
        {
            var client = factory.CreateClient();
            var newToken = await RefreshTokenAsync(client);

            Assert.NotEmpty(newToken);
        }

        [Fact]
        //[Fact(Skip = "Check Credentials Controller value and compare it with delay here.")]
        public async void WhenValidTokenSentButDelayedAfterExpiry_ReturnUnauthorized()
        {
            var client = factory.CreateClient();
            var oldToken = await LoginTests.GetTokenAsValidUserAsync(client);
            await Task.Delay(6000);
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(oldToken);

            var uri = SharedKernel.Configuration.Api.GetAppSetting().Token;
            var requestDto = new RequestDto<object>("DeviceId", null);

            var response = await client.PostAsJsonAsync(uri, requestDto);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async void WhenValidTokenSentFromDifferentDevice_ReturnUnauthorized()
        {
            var client = factory.CreateClient();
            var firstToken = await LoginTests.GetTokenAsValidUserAsync(client);
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(firstToken);

            var uri = SharedKernel.Configuration.Api.GetAppSetting().Token;
            var requestDto = new RequestDto<object>("DeviceId_", null);

            var response = await client.PostAsJsonAsync(uri, requestDto);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async void WhenNoTokenSent_ReturnUnauthorized()
        {
            var client = factory.CreateClient();

            var uri = SharedKernel.Configuration.Api.GetAppSetting().Token;
            var requestDto = new RequestDto<object>("DeviceId", null);

            var response = await client.PostAsJsonAsync(uri, requestDto);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async void WhenInValidTokenSent_ReturnUnauthorized()
        {
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bla");

            var uri = SharedKernel.Configuration.Api.GetAppSetting().Token;
            var requestDto = new RequestDto<object>("DeviceId", null);

            var response = await client.PostAsJsonAsync(uri, requestDto);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        public async static Task<string> RefreshTokenAsync(HttpClient client)
        {
            var oldToken = await LoginTests.GetTokenAsValidUserAsync(client);
            return await RefreshTokenAsync(client, oldToken);
        }

        private async static Task<string> RefreshTokenAsync(HttpClient client, string oldToken)
        {
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(oldToken);

            var uri = SharedKernel.Configuration.Api.GetAppSetting().Token;
            var requestDto = new RequestDto<object>("DeviceId", null);

            await Task.Delay(1500);
            var response = await client.PostAsJsonAsync(uri, requestDto);

            response.EnsureSuccessStatusCode();

            var responseDto = await response.Content.ReadFromJsonAsync<ResponseDto<string>>();

            return responseDto.Payload;
        }
    }
}
