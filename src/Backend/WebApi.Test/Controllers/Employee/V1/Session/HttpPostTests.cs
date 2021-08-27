using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using WeeControl.SharedKernel.Aggregates.Employee.DtosV1;
using WeeControl.SharedKernel.Common;
using WeeControl.SharedKernel.DtosV1;
using Xunit;

namespace WeeControl.Backend.WebApi.Test.Controllers.Employee.V1.Session
{
    public class HttpPostTests : IClassFixture<WebApplicationFactory<Startup>>, IDisposable
    {
        readonly IHttpMessage httpMessage;
        HttpRequestMessage request;

        public HttpPostTests(WebApplicationFactory<Startup> factory)
        {
            var client = factory.CreateClient();

            httpMessage = new HttpMessage(client, typeof(HttpPostTests).Namespace);

            request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                Version = new Version("1.0"),
                RequestUri = httpMessage.GetUri(ApiRouteEnum.EmployeeSession)
            };
        }

        public void Dispose()
        {
            request = null;
        }

        [Fact]
        public async void WhenPostingInvalidDto_ResponseIsBadRequest()
        {
            request.Content = httpMessage.GetHttpContentAsJson(new CreateLoginDto());

            var response = await httpMessage.GetResponseMessageAsync(request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async void WhenPostingInvalidPayloadDto_ResponseIsBadRequest()
        {
            request.Content = httpMessage.GetHttpContentAsJson(new RequestDto<string>() { Payload = "" });

            var response = await httpMessage.GetResponseMessageAsync(request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [InlineData("admin", "admin")]
        [InlineData("user", "user")]
        public async void LoginWithCorrectCredentials_ReturnOk(string username, string password)
        {
            request.Content = httpMessage.GetHttpContentAsJson(new RequestDto<CreateLoginDto>()
            {
                DeviceId = httpMessage.DeviceId,
                Payload = new CreateLoginDto()
                {
                    Username = username,
                    Password = password
                }
            });

            var response = await httpMessage.GetResponseMessageAsync(request);
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
            request.Content = httpMessage.GetHttpContentAsJson(new RequestDto<CreateLoginDto>()
            {
                DeviceId = httpMessage.DeviceId,
                Payload = new CreateLoginDto()
                {
                    Username = username,
                    Password = password
                }
            });

            var response = await httpMessage.GetResponseMessageAsync(request);

            Assert.Equal(statusCode, response.StatusCode);
        }
    }
}
