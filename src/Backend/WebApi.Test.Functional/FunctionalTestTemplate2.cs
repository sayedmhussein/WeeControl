using System;
using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using WeeControl.Common.BoundedContext.Credentials.DataTransferObjects;
using WeeControl.Common.BoundedContext.Credentials.Operations;
using WeeControl.Common.BoundedContext.RequestsResponses;
using Xunit;

namespace WeeControl.Backend.WebApi.Test.Functional
{
    public class FunctionalTestTemplate2 : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> factory;

        public FunctionalTestTemplate2(WebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
        }

        [Fact]
        public async void Test()
        {
            var client = factory.CreateClient();
            var requestDto = new RequestDto<RegisterDto>()
            {
                DeviceId = "bbb", Payload = new RegisterDto() { Username = "bbb", Password = "bbb" }
            };
            var content = new StringContent(JsonConvert.SerializeObject(requestDto), Encoding.UTF8, "application/json");

            var response = await client.PostAsync(ApiRouteLink.Register.Relative, content);

            response.EnsureSuccessStatusCode();
        }
    }
}
