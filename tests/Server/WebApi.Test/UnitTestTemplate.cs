using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace WeeControl.Server.WebApi.Test
{
    public class UnitTestTemplate : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> factory;

        public UnitTestTemplate(WebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
        }

        [Fact]
        public void Example()
        {
            Assert.NotNull(factory.CreateClient());
        }
    }
}
