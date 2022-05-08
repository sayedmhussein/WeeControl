﻿using System;
using System.Threading;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using WeeControl.Backend.Application.CommonContext.Queries;
using WeeControl.Backend.Application.EssentialContext.Commands;
using WeeControl.Backend.Application.EssentialContext.Queries;
using WeeControl.Backend.Application.Exceptions;
using WeeControl.Backend.Domain.Databases.Databases;
using WeeControl.Backend.Persistence;
using WeeControl.Common.SharedKernel.DataTransferObjects.Authorization.User;
using WeeControl.Common.SharedKernel.RequestsResponses;
using Xunit;

namespace WeeControl.Backend.Application.Test.EssentialContext.Commands
{
    public class RegisterCommandTests : IDisposable
    {
        private readonly IEssentialDbContext context;
        private Mock<IMediator> mediatorMock;

        public RegisterCommandTests()
        {
            context = new ServiceCollection().AddPersistenceAsInMemory().BuildServiceProvider().GetService<IEssentialDbContext>();
            mediatorMock = new Mock<IMediator>();
        }
        
        public void Dispose()
        {
            mediatorMock = null;
        }

        [Fact]
        public async void WhenRegisterNewUser_ReturnSuccessAndToken()
        {
            mediatorMock.Setup(x => x.Send(It.IsAny<GetNewTokenQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new ResponseDto<TokenDto>() { Payload = new TokenDto() { Token = "Token"}});
            var command = new RegisterCommand(new RequestDto() { DeviceId = "device" }, new RegisterDto() { Username = "username",Password = "password" });
            
            var tokenDto = await new RegisterHandler(context, mediatorMock.Object).Handle(command, default);

            mediatorMock.Verify(mock => mock.Send(It.IsAny<VerifyRequestQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            Assert.NotEmpty(tokenDto.Token);
        }

        [Fact]
        public async void WhenRegisterExistingUser_ThrowException()
        {
            var user = await context.Users.FirstOrDefaultAsync();
            var command = new RegisterCommand(new RequestDto() { DeviceId = "device" }, new RegisterDto() { Username = user.Username, Password = user.Password });

            await Assert.ThrowsAsync<ConflictFailureException>(() => new RegisterHandler(context, mediatorMock.Object).Handle(command, default));
        }

        [Theory]
        [InlineData(null, null, "password")]
        [InlineData("", "", "password")]
        [InlineData("username", "email", "")]
        public async void WhenInvalidInputs_ThrowException(string username, string email, string password)
        {
            var command = new RegisterCommand(new RequestDto() { DeviceId = "device" }, new RegisterDto() { Username = username, Email = email, Password = password });

            await Assert.ThrowsAnyAsync<ValidationException>(() => new RegisterHandler(context, mediatorMock.Object).Handle(command, default));
        }

        
    }
}