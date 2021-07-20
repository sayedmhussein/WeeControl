using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using WeeControl.SharedKernel.BasicSchemas.Common.DtosV1;
using WeeControl.SharedKernel.BasicSchemas.Common.Enums;
using WeeControl.SharedKernel.BasicSchemas.Employee.DtosV1;
using Xunit;

namespace WeeControl.Server.WebApi.Test.BasicV1FunctionalTests.Employee
{
    public class EmployeeSessionTests : BaseFunctionalTest, IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly Uri RequstUri;

        public EmployeeSessionTests(WebApplicationFactory<Startup> factory)
            : base(factory.CreateClient())
        {
            RequstUri = new Uri(new Uri(ApiRoute[ApiRouteEnum.Base]), ApiRoute[ApiRouteEnum.EmployeeSession]);
        }

        [Theory]
        [InlineData("admin", "admin", HttpStatusCode.OK)]
        [InlineData("user", "user", HttpStatusCode.OK)]
        [InlineData(null, null, HttpStatusCode.BadRequest)]
        [InlineData("", null, HttpStatusCode.BadRequest)]
        [InlineData(null, "", HttpStatusCode.BadRequest)]
        [InlineData("", "", HttpStatusCode.BadRequest)]
        [InlineData("bla", "bla", HttpStatusCode.NotFound)]
        [InlineData("admin1", "admin1", HttpStatusCode.NotFound)]
        public async void LoginTheoryTests(string username, string password, HttpStatusCode statusCode)
        {
            var loginDto = new CreateLoginDto()
            {
                Username = username,
                Password = password,
                Metadata = new RequestMetadataV1()
                {
                    Device = typeof(EmployeeSessionTests).Namespace
                }
            };

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                Version = new Version("1.0"),
                Content = GetHttpContentAsJson(loginDto),
                RequestUri = RequstUri
            };

            var response = await GetResponseMessageAsync(request);

            Assert.Equal(statusCode, response.StatusCode);

            if (statusCode == HttpStatusCode.OK)
            {
                response.EnsureSuccessStatusCode();

                var tokenDto = await response.Content.ReadFromJsonAsync<EmployeeTokenDto>();

                Assert.NotEmpty(tokenDto.Token);
            }
        }

        [Theory]
        [InlineData(false, false, HttpStatusCode.OK)]
        [InlineData(true, false, HttpStatusCode.Unauthorized)]
        [InlineData(false, true, HttpStatusCode.Forbidden)]
        [InlineData(true, true, HttpStatusCode.Unauthorized)]
        public async void RefreshTokenTheoryTests(bool changeToken, bool changeMetadata, HttpStatusCode statusCode)
        {
            var dto = new CreateLoginDto() { Username = "admin", Password = "admin", Metadata = Metadata };
            await CreateTokenAsync(dto);

            if (changeToken)
            {
                Token = new Random().NextDouble().ToString();
            }

            if (changeMetadata)
            {
                Metadata = new RequestMetadataV1() { Device = new Random().NextDouble().ToString() };
            }

            var loginDto = new RefreshLoginDto()
            {
                Metadata = Metadata
            };

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                Version = new Version("1.0"),
                Content = GetHttpContentAsJson(loginDto),
                RequestUri = RequstUri
            };

            var response = await GetResponseMessageAsync(request);

            Assert.Equal(statusCode, response.StatusCode);

            if (statusCode == HttpStatusCode.OK)
            {
                response.EnsureSuccessStatusCode();

                var tokenDto = await response.Content.ReadFromJsonAsync<EmployeeTokenDto>();

                Assert.NotEmpty(tokenDto.Token);
            }
        }

        [Theory]
        [InlineData(false, HttpStatusCode.OK)]
        [InlineData(true, HttpStatusCode.Unauthorized)]
        public async void LogoutTheoryTests(bool changeToken, HttpStatusCode statusCode)
        {
            var dto = new CreateLoginDto() { Username = "admin", Password = "admin", Metadata = Metadata };
            await CreateTokenAsync(dto);
            await RefreshTokenAsync();

            if (changeToken)
            {
                Token = new Random().NextDouble().ToString();
            }

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                Version = new Version("1.0"),
                RequestUri = RequstUri
            };

            var response = await GetResponseMessageAsync(request);

            Assert.Equal(statusCode, response.StatusCode);
        }
    }
}
