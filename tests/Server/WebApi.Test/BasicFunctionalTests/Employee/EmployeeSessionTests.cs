using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using WeeControl.SharedKernel.BasicSchemas.Common.Dicts;
using WeeControl.SharedKernel.BasicSchemas.Common.DtosV1;
using WeeControl.SharedKernel.BasicSchemas.Common.Enums;
using WeeControl.SharedKernel.BasicSchemas.Employee.DtosV1;
using Xunit;

namespace WeeControl.Server.WebApi.Test.FunctionalTests.Employee
{
    public class EmployeeSessionTests : BaseFunctionalTest, IClassFixture<WebApplicationFactory<Startup>>
    {
        private const string ADMIN_USERNAME = "admin";
        private const string ADMIN_PASSWORD = "admin";
        private readonly Uri RequstUri;

        public EmployeeSessionTests(WebApplicationFactory<Startup> factory) : base(factory.CreateClient(), EmployeeName.Admin)
        {
            var apiUris = new ApiDicts();
            var baseUri = new Uri(apiUris.ApiRoute[ApiRouteEnum.Base]);
            var relativeUri = new Uri(apiUris.ApiRoute[ApiRouteEnum.Employee] + "Session/", UriKind.Relative);

            RequstUri = new Uri(baseUri, relativeUri);
        }

        [Fact]
        public async void WhenLoginWithAdminCredentials_ReturnValidToken()
        {
            var request = GetLoginRequest(ADMIN_USERNAME, ADMIN_PASSWORD, new Random().NextDouble().ToString());

            var response = GetResponseMessage(request);
            response.EnsureSuccessStatusCode();

            var tokenDto = await response.Content.ReadFromJsonAsync<EmployeeTokenDto>();

            Assert.NotEmpty(tokenDto.Token);
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
            var request = GetLoginRequest(username, password, new Random().NextDouble().ToString());

            var response = GetResponseMessage(request);

            Assert.Equal(statusCode, response.StatusCode);
        }

        [Fact]
        public async void WhenRefreshTokenWithValidTokenAndSameDevice_ReturnNewToken()
        {
            var request1 = GetLoginRequest(ADMIN_USERNAME, ADMIN_PASSWORD, "device");
            var response1 = GetResponseMessage(request1);
            response1.EnsureSuccessStatusCode();
            var token1 = (await response1.Content.ReadFromJsonAsync<EmployeeTokenDto>()).Token;

            var request2 = GetTokenRequest("device");
            var response2 = GetResponseMessage(request2, token1);
            response2.EnsureSuccessStatusCode();
            var token2 = (await response2.Content.ReadFromJsonAsync<EmployeeTokenDto>()).Token;

            Assert.NotEmpty(token2);
        }

        [Fact]
        public async void WhenRefreshTokenWithValidTokenButDifferentDevice_ReturnForbidden()
        {
            var request1 = GetLoginRequest(ADMIN_USERNAME, ADMIN_PASSWORD, "device");
            var response1 = GetResponseMessage(request1);
            response1.EnsureSuccessStatusCode();
            var token1 = (await response1.Content.ReadFromJsonAsync<EmployeeTokenDto>()).Token;

            var request2 = GetTokenRequest("otherdevice");
            var response2 = GetResponseMessage(request2, token1);

            Assert.Equal(HttpStatusCode.Forbidden, response2.StatusCode);
        }

        [Fact]
        public void WhenRefreshTokenIsInvalid_ReturnUnAuthorized()
        {
            var request2 = GetTokenRequest("device");
            var response2 = GetResponseMessage(request2, "Invlaid");

            Assert.Equal(HttpStatusCode.Unauthorized, response2.StatusCode);
        }

        [Fact]
        public async void WhenLogout_NoErrorOccure()
        {
            var request1 = GetLoginRequest(ADMIN_USERNAME, ADMIN_PASSWORD, "device");
            var response1 = GetResponseMessage(request1);
            response1.EnsureSuccessStatusCode();
            var token1 = (await response1.Content.ReadFromJsonAsync<EmployeeTokenDto>()).Token;

            var request2 = GetTokenRequest("device");
            var response2 = GetResponseMessage(request2, token1);
            response2.EnsureSuccessStatusCode();
            var token2 = (await response2.Content.ReadFromJsonAsync<EmployeeTokenDto>()).Token;

            var request3 = GetLogoutRequest();
            var response3 = GetResponseMessage(request3, token2);

            Assert.Equal(HttpStatusCode.OK, response3.StatusCode);
        }

        private HttpRequestMessage GetLoginRequest(string username, string password, string device)
        {
            var loginDto = new CreateLoginDto()
            {
                Username = username,
                Password = password,
                Metadata = new RequestMetadata() { Device = device }
            };

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                Version = new Version("1.0"),
                Content = GetHttpContentAsJson(loginDto),
                RequestUri = RequstUri
            };

            return request;
        }

        private HttpRequestMessage GetTokenRequest(string device)
        {
            var loginDto = new RefreshLoginDto()
            {
                Metadata = new RequestMetadata() { Device = device }
            };

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                Version = new Version("1.0"),
                Content = GetHttpContentAsJson(loginDto),
                RequestUri = RequstUri
            };

            return request;
        }

        private HttpRequestMessage GetLogoutRequest()
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                Version = new Version("1.0"),
                RequestUri = RequstUri
            };

            return request;
        }

        //private HttpResponseMessage GetLoginHttpResponseMessage(string username, string password, string device)
        //{
        //    var client = factory.CreateClient();

        //    var metadata = new RequestMetadata() { Device = device };
        //    var loginDto = new CreateLoginDto() { Username = username, Password = password, Metadata = metadata };

        //    return client.PostAsJsonAsync(ROUTE, loginDto).GetAwaiter().GetResult();
        //}

        //private HttpResponseMessage GetNewTokenHttpResponseMessage(string token, string device)
        //{
        //    var client = factory.CreateClient();
        //    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(token);
        //    var metadata = new RequestMetadata() { Device = device };
        //    var loginDto = new RefreshLoginDto() { Metadata = metadata };

        //    return client.PutAsJsonAsync(ROUTE, loginDto).GetAwaiter().GetResult();
        //}

        //private HttpResponseMessage TerminateTokenHttpResponseMessage(string token, string device)
        //{
        //    var client = factory.CreateClient();
        //    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(token);

        //    return client.DeleteAsync(ROUTE).GetAwaiter().GetResult();
        //}
    }
}
