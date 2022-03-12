using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WeeControl.Backend.Application.BoundContexts.Credentials.Commands;
using WeeControl.Backend.Application.Exceptions;
using WeeControl.Backend.Domain.Credentials;
using WeeControl.Backend.Persistence;
using WeeControl.Common.BoundedContext.Credentials.DataTransferObjects;
using WeeControl.Common.BoundedContext.RequestsResponses;
using Xunit;

namespace WeeControl.Backend.Application.Test.BoundedContexts.Credentials
{
    public class RegisterCommandTests
    {
        private readonly ICredentialsDbContext context;

        public RegisterCommandTests()
        {
            context = new ServiceCollection().AddPersistenceAsInMemory().BuildServiceProvider().GetService<ICredentialsDbContext>();
        }

        [Fact]
        public async void WhenRegisterNewUser_ReturnSuccessAndToken()
        {
            var command = new RegisterCommand(new RequestDto() { DeviceId = "device" }, new RegisterDto() { Username = "username", Password = "password" });

            var tokenDto = await new RegisterHandler(context).Handle(command, default);

            Assert.NotEmpty(tokenDto.Token);
        }

        [Fact]
        public async void WhenRegisterExistingUser_ReturnSuccessAndToken()
        {
            var user = await context.Users.FirstOrDefaultAsync();
            var command = new RegisterCommand(new RequestDto() { DeviceId = "device" }, new RegisterDto() { Username = user.Username, Password = user.Password });

            await Assert.ThrowsAsync<ConflictFailureException>(() => new RegisterHandler(context).Handle(command, default));
        }

        [Theory]
        [InlineData(null, "username", "email", "password")]
        [InlineData("device", null, null, "password")]
        [InlineData("device", "", "", "password")]
        [InlineData("device", "username", "email", "")]
        public async void WhenInvalidInputs_ThrowExceptions(string device, string username, string email, string password)
        {
            var user = await context.Users.FirstOrDefaultAsync();
            var command = new RegisterCommand(new RequestDto() { DeviceId = device }, new RegisterDto() { Username = username, Email = email, Password = password });

            await Assert.ThrowsAnyAsync<Exception>(() => new RegisterHandler(context).Handle(command, default));
        }
    }
}
