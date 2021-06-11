using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using MySystem.SharedKernel.EntityV1Dtos.Common;
using MySystem.SharedKernel.EntityV1Dtos.Employee;
using MySystem.SharedKernel.Enumerators;
using MySystem.SharedKernel.Interfaces;
using MySystem.SharedKernel.Services;
using Xunit;

namespace MySystem.Web.Api.Test.FunctionalTests.Employee
{
    public class EmployeeSessionTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private const string ADMIN_USERNAME = "admin";
        private const string ADMIN_PASSWORD = "admin";
        private readonly string ROUTE;

        private readonly WebApplicationFactory<Startup> factory;

        public EmployeeSessionTests(WebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
            var sharedValues = new SharedValues();
            ROUTE = sharedValues.ApiRoute[ApiRouteEnum.Employee] + "Session/";
        }

        [Fact]
        public async void WhenLoginWithAdminCredentials_ReturnValidToken()
        {
            var response = GetLoginHttpResponseMessage(ADMIN_USERNAME, ADMIN_PASSWORD, new Random().NextDouble().ToString());

            response.EnsureSuccessStatusCode();

            var tokenDto = await response.Content.ReadFromJsonAsync<EmployeeTokenDto>();
            var token = tokenDto.Token;

            Assert.NotEmpty(token);
        }

        [Theory]
        [InlineData(null, null, HttpStatusCode.BadRequest)]
        [InlineData("", null, HttpStatusCode.BadRequest)]
        [InlineData(null, "", HttpStatusCode.BadRequest)]
        [InlineData("", "", HttpStatusCode.BadRequest)]
        [InlineData("bla", "bla", HttpStatusCode.NotFound)]
        [InlineData("admin1", "admin1", HttpStatusCode.NotFound)]
        public void WhenLoginWithInvalidCredentials_Return(string username, string password, HttpStatusCode statusCode)
        {
            var response = GetLoginHttpResponseMessage(username, password, new Random().NextDouble().ToString());

            Assert.Equal(statusCode, response.StatusCode);
        }

        [Fact]
        public async void WhenRefreshTokenWithValidTokenAndSameDevice_ReturnNewToken()
        {
            var token1Response = GetLoginHttpResponseMessage(ADMIN_USERNAME, ADMIN_PASSWORD, "device");
            token1Response.EnsureSuccessStatusCode();
            var token1Dto = await token1Response.Content.ReadFromJsonAsync<EmployeeTokenDto>();

            var token2Response = GetNewTokenHttpResponseMessage(token1Dto.Token, "device");
            token2Response.EnsureSuccessStatusCode();
            var token2Dto = await token2Response.Content.ReadFromJsonAsync<EmployeeTokenDto>();

            Assert.NotEmpty(token2Dto.Token);
        }

        [Fact]
        public async void WhenRefreshTokenWithValidTokenButDifferentDevice_ReturnForbidden()
        {
            var token1Response = GetLoginHttpResponseMessage(ADMIN_USERNAME, ADMIN_PASSWORD, "device");
            token1Response.EnsureSuccessStatusCode();
            var token1Dto = await token1Response.Content.ReadFromJsonAsync<EmployeeTokenDto>();

            var token2Response = GetNewTokenHttpResponseMessage(token1Dto.Token, "device1");

            Assert.Equal(HttpStatusCode.Forbidden, token2Response.StatusCode);
        }

        [Fact]
        public void WhenRefreshTokenIsInvalid_ReturnUnAuthorized()
        {
            var token2Response = GetNewTokenHttpResponseMessage(new Random().NextDouble().ToString(), "device1");

            Assert.Equal(HttpStatusCode.Unauthorized, token2Response.StatusCode);
        }

        [Fact]
        public async void WhenLogout_NoErrorOccure()
        {
            var token1Response = GetLoginHttpResponseMessage(ADMIN_USERNAME, ADMIN_PASSWORD, "device");
            token1Response.EnsureSuccessStatusCode();
            var token1Dto = await token1Response.Content.ReadFromJsonAsync<EmployeeTokenDto>();

            var token2Response = GetNewTokenHttpResponseMessage(token1Dto.Token, "device");
            token2Response.EnsureSuccessStatusCode();
            var token2Dto = await token2Response.Content.ReadFromJsonAsync<EmployeeTokenDto>();

            var logoutResponse = TerminateTokenHttpResponseMessage(token2Dto.Token, "device");

            Assert.Equal(HttpStatusCode.OK, logoutResponse.StatusCode);
        }

        private HttpResponseMessage GetLoginHttpResponseMessage(string username, string password, string device)
        {
            var client = factory.CreateClient();

            var metadata = new RequestMetadata() { Device = device };
            var loginDto = new CreateLoginDto() { Username = username, Password = password, Metadata = metadata };

            return client.PostAsJsonAsync(ROUTE, loginDto).GetAwaiter().GetResult();
        }

        private HttpResponseMessage GetNewTokenHttpResponseMessage(string token, string device)
        {
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(token);
            var metadata = new RequestMetadata() { Device = device };
            var loginDto = new RefreshLoginDto() { Metadata = metadata };

            return client.PutAsJsonAsync(ROUTE, loginDto).GetAwaiter().GetResult();
        }

        private HttpResponseMessage TerminateTokenHttpResponseMessage(string token, string device)
        {
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(token);

            return client.DeleteAsync(ROUTE).GetAwaiter().GetResult();
        }
    }
}
