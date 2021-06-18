using System;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace WeeControl.Server.WebApi.Test
{
    public class BaseFunctionalTestTester : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> factory;

        public BaseFunctionalTestTester(WebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
        }

        [Fact]
        public void WhenGetTokenOfAdmin_TokenStringNotNull()
        {
            var client = factory.CreateClient();

            var token = new BaseFunctionalTest(client, EmployeeName.Admin).Token;

            Assert.NotEmpty(token);
        }

        [Fact]
        public void WhenGetTokenOfUser_TokenStringNotNull()
        {
            var client = factory.CreateClient();

            var token = new BaseFunctionalTest(client, EmployeeName.User).Token;

            Assert.NotEmpty(token);
        }
    }
}
