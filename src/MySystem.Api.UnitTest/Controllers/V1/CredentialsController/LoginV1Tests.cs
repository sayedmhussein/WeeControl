using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using MySystem.ServerData;
using MySystem.ServerData.Models.People;
using MySystem.SharedDto.V1;
using MySystem.SharedDto.V1.Custom;
using Xunit;

namespace MySystem.Api.UnitTest.Controllers.V1.CredentialsController
{
    public class LoginV1Tests
    {
        private readonly Mock<IConfiguration> configMock;
        private readonly Mock<ILogger<Api.Controllers.V1.CredentialsController>> loggerMock;
        private readonly DataContext context;

        public LoginV1Tests()
        {
            configMock = new Mock<IConfiguration>();
            configMock.Setup(x => x["Jwt:Key"]).Returns("MySystem.Api.UnitTest.Controllers.V1.CredentialsController");

            loggerMock = new Mock<ILogger<Api.Controllers.V1.CredentialsController>>();

            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "mydbname")
                .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;
            context = new DataContext(options);
        }

        [Fact]
        public async void LoginWithCorrectCredentials_ReturnOk()
        {
            var controller = new MySystem.Api.Controllers.V1.CredentialsController(loggerMock.Object, context, configMock.Object);
            var dto = new RequestDto<LoginDto>(new LoginDto() { Username = "username", Password = "password" });

            var response = await controller.LoginV1(dto);

            var result = Assert.IsType<OkObjectResult>(response.Result);
            Assert.Equal(HttpStatusCode.OK, (HttpStatusCode)result.StatusCode);
        }

        [Fact]
        public async void LoginWithInvalidCredentials_ReturnNotFound()
        {
            var controller = new MySystem.Api.Controllers.V1.CredentialsController(loggerMock.Object, context, configMock.Object);
            var dto = new RequestDto<LoginDto>(new LoginDto() { Username = "bla", Password = "password" });

            var response = await controller.LoginV1(dto);

            var result = Assert.IsType<NotFoundResult>(response.Result);
            Assert.Equal(HttpStatusCode.NotFound, (HttpStatusCode)result.StatusCode);
        }

        [Fact]
        public async void LoginWithInvalidDto_ReturnBadRequest()
        {
            var controller = new MySystem.Api.Controllers.V1.CredentialsController(loggerMock.Object, context, configMock.Object);
            var dto = new RequestDto<LoginDto>(new LoginDto() { Username = null, Password = null });

            var response = await controller.LoginV1(dto);

            var result = Assert.IsType<BadRequestResult>(response.Result);
            Assert.Equal(HttpStatusCode.BadRequest, (HttpStatusCode)result.StatusCode);
        }
    }
}