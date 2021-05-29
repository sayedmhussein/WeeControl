using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using MySystem.Persistence.Shared.Configuration;
using MySystem.Persistence.Shared.Dtos.V1;
using MySystem.Persistence.Shared.Dtos.V1.Custom;
using Xunit;

namespace MySystem.Persistence.Api.Test.FunctionalTest.ControllersV1
{
    public class LoginTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> factory;

        public LoginTests(WebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
        }

        [Fact]
        public async void WhenValidCredentials_NewTokenCreated()
        {
            var client = factory.CreateClient();
            var token = await GetTokenAsValidUserAsync(client);
            Assert.NotEmpty(token);
        }

        [Fact]
        public async void WhenInValidCredentials_NotFoundStatuesCode()
        {
            var client = factory.CreateClient();
            var uri = AppSettings.GetAppSetting().Api.Login;
            var loginDto = new LoginDto() { Username = "sayed", Password = "something" };
            var requestDto = new RequestDto<LoginDto>("DeviceId", loginDto);

            var response = await client.PostAsJsonAsync(uri, requestDto);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async void WhenInValidRequstDto_BadRequestStatuesCode()
        {
            var client = factory.CreateClient();
            var uri = AppSettings.GetAppSetting().Api.Login;

            var requestDto = new RequestDto<string>("DeviceId", "something");

            var response = await client.PostAsJsonAsync(uri, requestDto);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        internal static async Task<string> GetTokenAsValidUserAsync(HttpClient client)
        {
            var uri = AppSettings.GetAppSetting().Api.Login;
            var loginDto = new LoginDto() { Username = "sayed", Password = "sayed" };
            var requestDto = new RequestDto<LoginDto>("DeviceId", loginDto);

            var response = await client.PostAsJsonAsync(uri, requestDto);

            response.EnsureSuccessStatusCode();

            var responseDto = await response.Content.ReadFromJsonAsync<ResponseDto<string>>();
            return responseDto.Payload;
        }
    }
}
