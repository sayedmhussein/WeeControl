using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using WeeControl.SharedKernel.DtosV1;
using WeeControl.SharedKernel.DtosV1.Employee;
using WeeControl.SharedKernel.Helpers;
using Xunit;

namespace WeeControl.Backend.WebApi.Test.Functional.V1.Employee.Session
{
    public class HttpPostTests :
        BaseFunctionalTest,
        IClassFixture<CustomWebApplicationFactory<Startup>>,
        IDisposable
    {
        public HttpPostTests(CustomWebApplicationFactory<Startup> factory) :
            base(factory, HttpMethod.Post, typeof(HttpPostTests).Namespace)
        {
            ServerUri = GetUri(ApiRouteEnum.EmployeeSession);
        }

        [Fact]
        public async void WhenPostingInvalidDto_ResponseIsBadRequest()
        {
            var content = GetHttpContentAsJson(new CreateLoginDto());

            var response = await GetResponseMessageAsync(ServerUri, content);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async void WhenPostingInvalidPayloadDto_ResponseIsBadRequest()
        {
            var content = GetHttpContentAsJson(new RequestDto<string>() { Payload = "" });

            var response = await GetResponseMessageAsync(ServerUri, content);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [InlineData("admin", "admin")]
        [InlineData("user", "user")]
        public async void LoginWithCorrectCredentials_ReturnOk(string username, string password)
        {
            var content = GetHttpContentAsJson(new RequestDto<CreateLoginDto>()
            {
                DeviceId = DeviceId,
                Payload = new CreateLoginDto()
                {
                    Username = username,
                    Password = password
                }
            });

            var response = await GetResponseMessageAsync(ServerUri, content);
            var tokenDto = await response.Content.ReadFromJsonAsync<ResponseDto<EmployeeTokenDto>>();
            Assert.NotEmpty(tokenDto.Payload.Token);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Theory]
        [InlineData(null, null, HttpStatusCode.BadRequest)]
        [InlineData("", null, HttpStatusCode.BadRequest)]
        [InlineData(null, "", HttpStatusCode.BadRequest)]
        [InlineData("", "", HttpStatusCode.BadRequest)]
        [InlineData("bla", "bla", HttpStatusCode.NotFound)]
        [InlineData("admin1", "admin1", HttpStatusCode.NotFound)]
        public async void LoginTheoryTests(string username, string password, HttpStatusCode statusCode)
        {
            var content = GetHttpContentAsJson(new RequestDto<CreateLoginDto>()
            {
                DeviceId = DeviceId,
                Payload = new CreateLoginDto()
                {
                    Username = username,
                    Password = password
                }
            });

            var response = await GetResponseMessageAsync(ServerUri, content);

            Assert.Equal(statusCode, response.StatusCode);
        }
    }
}
