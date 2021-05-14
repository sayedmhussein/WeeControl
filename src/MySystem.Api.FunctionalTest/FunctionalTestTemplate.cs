using System;
using Microsoft.AspNetCore.Mvc.Testing;
using Sayed.MySystem.Api;
using Xunit;

namespace Sayed.MySystem.Api.FunctionalTest
{
    public class FunctionalTestTemplate : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> factory;

        public FunctionalTestTemplate(WebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
        }

        [Fact]
        public void Test1()
        {
            Assert.NotNull(factory.CreateClient());
        }
    }
}
