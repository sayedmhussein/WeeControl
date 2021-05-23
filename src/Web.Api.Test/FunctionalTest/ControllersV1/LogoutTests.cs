using System;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace MySystem.Web.Api.Test.FunctionalTest.ControllersV1
{
    public class LogoutTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> factory;

        public LogoutTests(WebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
        }
    }
}
