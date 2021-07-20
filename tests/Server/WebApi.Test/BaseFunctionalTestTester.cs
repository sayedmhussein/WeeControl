using System;
using Microsoft.AspNetCore.Mvc.Testing;
using WeeControl.SharedKernel.BasicSchemas.Common.DtosV1;
using WeeControl.SharedKernel.BasicSchemas.Employee.DtosV1;
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

        [Theory]
        [InlineData("admin", "admin")]
        [InlineData("user", "user")]
        public async void GetToken_TokenMustNotBeEmpty(string username, string password)
        {
            var baseTestClass = new BaseFunctionalTest(factory.CreateClient());
            var metadata = new RequestMetadataV1() { Device = nameof(BaseFunctionalTestTester) };
            var dto = new CreateLoginDto()
            { Username = username, Password = password, Metadata = metadata };

            await baseTestClass.CreateTokenAsync(dto);
            await baseTestClass.RefreshTokenAsync();

            Assert.NotEmpty(baseTestClass.Token);
        }
    }
}
