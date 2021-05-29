using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using MySystem.Persistence.Shared.Dtos.V1;
using MySystem.Persistence.Shared.Dtos.V1.Custom;
using Xunit;
using MySystem.Persistence.Api.Controllers.V1;
using MySystem.Persistence.Api.UnitTest;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace MySystem.Persistence.Api.Test.UnitTest.Controllers.V1.Credentials
{
    public class LoginV1Tests : IDisposable
    {
        private CredentialsController controller;

        public LoginV1Tests()
        {
            controller = new CredentialsController(TestObjects.DataContext, TestObjects.Configuration, null, null);
        }

        public void Dispose()
        {
            controller = null;
        }

        [Fact]
        public async void LoginWithCorrectCredentials_ReturnOk()
        {
            var dto = new RequestDto<LoginDto>("DeviceId", new LoginDto() { Username = "sayed", Password = "sayed" });

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
            var dto = new RequestDto<LoginDto>(new LoginDto() { Username = null, Password = "password" });

            var response = await controller.LoginV1(dto);

            var result = Assert.IsType<BadRequestObjectResult>(response.Result);
            Assert.Equal(HttpStatusCode.BadRequest, (HttpStatusCode)result.StatusCode);
        }
    }
}