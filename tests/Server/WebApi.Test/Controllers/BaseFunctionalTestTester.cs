//using Microsoft.AspNetCore.Mvc.Testing;
//using WeeControl.SharedKernel.Aggregates.Employee.DtosV1;
//using Xunit;

//namespace WeeControl.Server.WebApi.Test.Controllers
//{
//    public class BaseFunctionalTestTester : IClassFixture<WebApplicationFactory<Startup>>
//    {
//        private readonly WebApplicationFactory<Startup> factory;

//        public BaseFunctionalTestTester(WebApplicationFactory<Startup> factory)
//        {
//            this.factory = factory;
//        }

//        [Theory]
//        //[InlineData("admin", "admin")]
//        [InlineData("user", "user")]
//        public async void GetToken_TokenMustNotBeEmpty(string username, string password)
//        {
//            var baseTestClass = new BaseFunctionalTest(factory.CreateClient());
//            var dto = new CreateLoginDto()
//            { Username = username, Password = password };

//            await baseTestClass.CreateTokenAsync(dto);
//            await baseTestClass.RefreshTokenAsync();

//            Assert.NotEmpty(baseTestClass.Token);
//        }
//    }
//}
