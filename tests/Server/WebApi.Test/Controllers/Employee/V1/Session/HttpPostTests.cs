using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using WeeControl.SharedKernel.Aggregates.Employee.DtosV1;
using WeeControl.SharedKernel.Common;
using WeeControl.SharedKernel.DtosV1;
using Xunit;

namespace WeeControl.Server.WebApi.Test.Controllers.Employee.V1.Session
{
    public class HttpPostTests : FunctionalBaseTest<CreateLoginDto>, IClassFixture<WebApplicationFactory<Startup>>
    {
        public HttpPostTests(WebApplicationFactory<Startup> factory)
            : base(factory.CreateClient(), "1.0", HttpMethod.Post, ApiRouteEnum.EmployeeSession)
        {
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
            RequestDto = new RequestDto<CreateLoginDto>()
            {
                DeviceId = typeof(HttpPostTests).Namespace,
                Payload = new CreateLoginDto()
                {
                    Username = username,
                    Password = password
                }
            };

            var token = await new FunctionalAuthorization(client).GetTokenAsync("admin", "admin", "");

            var response = await GetResponseMessageAsync(token);

            Assert.Equal(statusCode, response.StatusCode);

            if (statusCode == HttpStatusCode.OK)
            {
                response.EnsureSuccessStatusCode();

                var tokenDto = await response.Content.ReadFromJsonAsync<EmployeeTokenDto>();

                Assert.NotEmpty(tokenDto.Token);
            }
        }
    }
}
