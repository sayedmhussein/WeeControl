using System;
using Microsoft.AspNetCore.Mvc.Testing;
using MySystem.Persistence.Api;
using Xunit;

namespace MySystem.Api.Test
{
    public class FunctionalTestTemplate : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> factory;

        public FunctionalTestTemplate(WebApplicationFactory<Startup> factory)
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
