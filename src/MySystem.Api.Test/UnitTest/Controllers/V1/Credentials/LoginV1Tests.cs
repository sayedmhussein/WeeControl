using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Sayed.MySystem.Shared.Dtos.V1;
using Sayed.MySystem.Shared.Dtos.V1.Custom;
using Xunit;
using Sayed.MySystem.Api.Controllers.V1;

namespace Sayed.MySystem.Api.UnitTest.Controllers.V1.Credentials
{
    public class LoginV1Tests : IDisposable
    {
        private ILogger<CredentialsController> logger;
        private CredentialsController controller;

        public LoginV1Tests()
        {
            logger = new Mock<ILogger<CredentialsController>>().Object;
            controller = new CredentialsController(logger, TestObjects.DataContext, TestObjects.Configuration, TestObjects.HttpContextAccessor);
        }

        public void Dispose()
        {
            controller = null;
            logger = null;
        }

        [Fact]
        public async void LoginWithCorrectCredentials_ReturnOk()
        {
            var dto = new RequestDto<LoginDto>(new LoginDto() { Username = "sayed", Password = "sayed" });

            var response = await controller.LoginV1(dto);

            var result = Assert.IsType<OkObjectResult>(response.Result);
            Assert.Equal(HttpStatusCode.OK, (HttpStatusCode)result.StatusCode);
        }

        [Fact]
        public async void LoginWithInvalidCredentials_ReturnNotFound()
        {
            var dto = new RequestDto<LoginDto>(new LoginDto() { Username = "bla", Password = "password" });

            var response = await controller.LoginV1(dto);

            var result = Assert.IsType<NotFoundResult>(response.Result);
            Assert.Equal(HttpStatusCode.NotFound, (HttpStatusCode)result.StatusCode);
        }

        [Fact]
        public async void LoginWithInvalidDto_ReturnBadRequest()
        {
            var dto = new RequestDto<LoginDto>(new LoginDto() { Username = null, Password = null });

            var response = await controller.LoginV1(dto);

            var result = Assert.IsType<BadRequestResult>(response.Result);
            Assert.Equal(HttpStatusCode.BadRequest, (HttpStatusCode)result.StatusCode);
        }
    }
}