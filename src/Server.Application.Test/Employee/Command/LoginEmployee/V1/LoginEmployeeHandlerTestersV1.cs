using System;
using Xunit;
using Moq;
using MySystem.Application.Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using MySystem.Persistence;
using Application.Employee.Command.LoginEmployee.V1;
using MySystem.SharedKernel.Dto.V1;
using System.Collections.Generic;
using System.Security.Claims;
using Application.Common.Exceptions;

namespace MySystem.Application.Test.Employee.Command.LoginEmployee.V1
{
    public class LoginEmployeeHandlerTesters : IDisposable
    {
        private IMySystemDbContext dbContext;
        private IJwtService jwtService;

        public LoginEmployeeHandlerTesters()
        {
            dbContext = new ServiceCollection().AddPersistenceAsInMemory(null).BuildServiceProvider().GetService<IMySystemDbContext>();

            var jwtMock = new Mock<IJwtService>();
            jwtMock.Setup(x => x.GenerateJwtToken(It.IsAny<IEnumerable<Claim>>(), It.IsAny<string>(), It.IsAny<DateTime>()))
                .Returns("Token");

            jwtService = jwtMock.Object;
        }

        public void Dispose()
        {
            dbContext = null;
        }

        [Fact]
        public async void WhenValidUsernameAndPassword_ReturnNewToken()
        {
            LoginEmployeeCommand command = new() { DeviceId = "DeviceId", Payload = new LoginDto() { Username = "username", Password = "password" } };

            var response = await new LoginEmployeeHandler(dbContext, jwtService).Handle(command, default);

            Assert.NotEmpty(response.Payload);
        }

        [Fact]
        public async void WhenInValidUsernameAndPassword_ThrowNotFoundException()
        {
            LoginEmployeeCommand command = new() { DeviceId = "DeviceId", Payload = new LoginDto() { Username = "bla", Password = "password" } };

            await Assert.ThrowsAsync<NotFoundException>(async () => await new LoginEmployeeHandler(dbContext, jwtService).Handle(command, default));
        }

        [Fact]
        public async void WhenNullCommand_ThrowArgumentNullException()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await new LoginEmployeeHandler(dbContext, jwtService).Handle(null, default));
        }

        [Fact]
        public async void WhenNullRequestDto_ThrowArgumentNullException()
        {
            var command = new LoginEmployeeCommand() { DeviceId = "Device Id", Payload = null };
            await Assert.ThrowsAsync<BadRequestException>(async () => await new LoginEmployeeHandler(dbContext, jwtService).Handle(command, default));
        }

        [Fact]
        public async void WhenNullDeviceInRequestDto_ThrowArgumentNullException()
        {
            var command = new LoginEmployeeCommand() { DeviceId = null, Payload = new LoginDto() { Username = "bla", Password = "password" } };
            await Assert.ThrowsAsync<BadRequestException>(async () => await new LoginEmployeeHandler(dbContext, jwtService).Handle(command, default));
        }

        [Fact]
        public async void WhenNullUsernameInRequestDto_ThrowArgumentNullException()
        {
            var command = new LoginEmployeeCommand() { DeviceId = "Device Id", Payload = new LoginDto() { Username = null, Password = "password" } };
            await Assert.ThrowsAsync<BadRequestException>(async () => await new LoginEmployeeHandler(dbContext, jwtService).Handle(command, default));
        }

        [Fact]
        public async void WhenEmptyUsernameInRequestDto_ThrowArgumentNullException()
        {
            var command = new LoginEmployeeCommand() { DeviceId = "Device Id", Payload = new LoginDto() { Username = string.Empty, Password = "password" } };
            await Assert.ThrowsAsync<BadRequestException>(async () => await new LoginEmployeeHandler(dbContext, jwtService).Handle(command, default));
        }
    }
}
