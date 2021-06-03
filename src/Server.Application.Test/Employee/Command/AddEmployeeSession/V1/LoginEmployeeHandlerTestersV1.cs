using System;
using System.Collections.Generic;
using System.Security.Claims;
using Application.Employee.Query.GetNewToken.V1;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using MySystem.Application.Common.Exceptions;
using MySystem.Application.Common.Interfaces;
using MySystem.Application.Employee.Command.AddEmployeeSession.V1;
using MySystem.Persistence;
using MySystem.SharedKernel.Entities.Employee.V1Dto;
using Xunit;

namespace MySystem.Application.Test.Employee.Command.AddEmployeeSession.V1
{
    public class LoginEmployeeHandlerTesters : IDisposable
    {
        private IMySystemDbContext dbContext;
        private ICurrentUserInfo currentUserInfo;

        public LoginEmployeeHandlerTesters()
        {
            dbContext = new ServiceCollection().AddPersistenceAsInMemory(null).BuildServiceProvider().GetService<IMySystemDbContext>();

            var jwtMock = new Mock<ICurrentUserInfo>();

            currentUserInfo = jwtMock.Object;
        }

        public void Dispose()
        {
            dbContext = null;
        }

        [Fact]
        public async void WhenValidUsernameAndPassword_ReturnNewToken()
        {
            AddEmployeeSessionCommand command = new() { DeviceId = "DeviceId", Payload = new LoginDto() { Username = "username", Password = "password" } };

            var claims = await new AddEmployeeSessionHandler(dbContext, currentUserInfo).Handle(command, default);

            Assert.NotEmpty(claims);
        }

        [Fact]
        public async void WhenInValidUsernameAndPassword_ThrowNotFoundException()
        {
            AddEmployeeSessionCommand command = new() { DeviceId = "DeviceId", Payload = new LoginDto() { Username = "bla", Password = "password" } };

            await Assert.ThrowsAsync<NotFoundException>(async () => await new AddEmployeeSessionHandler(dbContext, currentUserInfo).Handle(command, default));
        }

        [Fact]
        public async void WhenNullCommand_ThrowArgumentNullException()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await new AddEmployeeSessionHandler(dbContext, currentUserInfo).Handle(null, default));
        }

        [Fact]
        public async void WhenNullRequestDto_ThrowArgumentNullException()
        {
            var command = new AddEmployeeSessionCommand() { DeviceId = "Device Id", Payload = null };
            await Assert.ThrowsAsync<BadRequestException>(async () => await new AddEmployeeSessionHandler(dbContext, currentUserInfo).Handle(command, default));
        }

        [Fact]
        public async void WhenNullDeviceInRequestDto_ThrowArgumentNullException()
        {
            var command = new GetNewTokenQuery() { DeviceId = null, Payload = new LoginDto() { Username = "bla", Password = "password" } };
            //await Assert.ThrowsAsync<BadRequestException>(async () => await new GetNewTokenHandler(dbContext, jwtService).Handle(command, default));
        }

        [Fact]
        public async void WhenNullUsernameInRequestDto_ThrowArgumentNullException()
        {
            var command = new GetNewTokenQuery() { DeviceId = "Device Id", Payload = new LoginDto() { Username = null, Password = "password" } };
            //await Assert.ThrowsAsync<BadRequestException>(async () => await new GetNewTokenHandler(dbContext, jwtService).Handle(command, default));
        }

        [Fact]
        public async void WhenEmptyUsernameInRequestDto_ThrowArgumentNullException()
        {
            var command = new GetNewTokenQuery() { DeviceId = "Device Id", Payload = new LoginDto() { Username = string.Empty, Password = "password" } };
            //await Assert.ThrowsAsync<BadRequestException>(async () => await new GetNewTokenHandler(dbContext, jwtService).Handle(command, default));
        }
    }
}
