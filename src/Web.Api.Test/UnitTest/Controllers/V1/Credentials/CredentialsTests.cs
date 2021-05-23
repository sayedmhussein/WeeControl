using System;
using MySystem.Web.Api.Controllers.V1;
using MySystem.Web.Api.UnitTest;
using MySystem.Web.Shared.Dtos.V1;
using MySystem.Web.Shared.Dtos.V1.Custom;
using Xunit;
namespace MySystem.Api.Test.UnitTest.Controllers.V1.Credentials
{
    public class CredentialsTests
    {
        [Fact]
        public async void WhenNullLogger_NoExceptionBeenThrowed()
        {
            var controller = new CredentialsController(TestObjects.DataContext, TestObjects.Configuration, null, null);

            await controller.LoginV1(new RequestDto<LoginDto>() { Payload = new LoginDto() {Username = "a", Password = "a" } });
        }
    }
}
