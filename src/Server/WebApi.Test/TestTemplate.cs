using Microsoft.AspNetCore.Mvc.Testing;
using MySystem.Web.Api;
using Xunit;

namespace MySystem.Api.Test
{
    public class TestTemplate : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> factory;

        public TestTemplate(WebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
        }

        [Fact]
        public void LoginWithValidCredentials_ReturnSuccessWithToken()
        {
            Assert.NotNull(factory.CreateClient());
        }
    }
}
