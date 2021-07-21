using System;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace WeeControl.Server.WebApi.Test
{
    public class FunctionalTestTemplate :
        BaseFunctionalTest,
        IClassFixture<WebApplicationFactory<Startup>>
    {
        public FunctionalTestTemplate(WebApplicationFactory<Startup> factory) :
            base(factory.CreateClient())
        {
        }

        [Fact]
        public void Example()
        {
            Assert.NotNull(RequestMetadata);
        }
    }
}
