using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using WeeControl.Backend.WebApi.Test.Functional.TestHelpers;
using WeeControl.Common.SharedKernel.DtosV1;
using WeeControl.Common.SharedKernel.DtosV1.Authorization;
using WeeControl.Common.SharedKernel.DtosV1.Common;
using WeeControl.Common.SharedKernel.DtosV1.Employee;
using WeeControl.Common.SharedKernel.Routing;
using Xunit;

namespace WeeControl.Backend.WebApi.Test.Functional.V1.Employee.Session
{
    public class HttpPostTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> factory;
        private readonly IFunctionalTest test;
        private readonly IFunctionalAuthorization authorization;
        private readonly Uri routeUri;
        
        public HttpPostTests(CustomWebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
            test = new FunctionalTest(factory, HttpMethod.Post, typeof(HttpPostTests).Namespace);
            authorization = new FunctionalAuthorization(test);
            routeUri = test.GetUri(ApiRouteEnum.EmployeeSession);
        }

        [Fact]
        public async void WhenPostingInvalidDto_ResponseIsBadRequest()
        {
            var content = test.GetHttpContentAsJson(new RequestNewTokenDto());

            var response = await test.GetResponseMessageAsync(routeUri, content);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async void WhenPostingInvalidPayloadDto_ResponseIsBadRequest()
        {
            var content = test.GetHttpContentAsJson(new RequestDto<string>() { Payload = "" });

            var response = await test.GetResponseMessageAsync(routeUri, content);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [InlineData("admin", "admin")]
        [InlineData("user", "user")]
        public async void LoginWithCorrectCredentials_ReturnOk(string username, string password)
        {
            var content = test.GetHttpContentAsJson(new RequestDto<RequestNewTokenDto>()
            {
                DeviceId = test.DeviceId,
                Payload = new RequestNewTokenDto()
                {
                    Username = username,
                    Password = password
                }
            });

            var response = await test.GetResponseMessageAsync(routeUri, content);
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
            var content = test.GetHttpContentAsJson(new RequestDto<RequestNewTokenDto>()
            {
                DeviceId = test.DeviceId,
                Payload = new RequestNewTokenDto()
                {
                    Username = username,
                    Password = password
                }
            });

            var response = await test.GetResponseMessageAsync(routeUri, content);

            Assert.Equal(statusCode, response.StatusCode);
        }
    }
}
