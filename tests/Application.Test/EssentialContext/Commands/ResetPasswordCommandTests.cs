using System;
using System.Linq;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using WeeControl.Application.Essential;
using WeeControl.Application.Essential.Commands;
using WeeControl.Application.Exceptions;
using WeeControl.Application.Interfaces;
using WeeControl.Domain.Essential.Entities;
using WeeControl.Persistence;
using WeeControl.SharedKernel.DataTransferObjects.User;
using WeeControl.SharedKernel.Interfaces;
using WeeControl.SharedKernel.RequestsResponses;
using WeeControl.SharedKernel.Services;
using Xunit;

namespace WeeControl.Application.Test.EssentialContext.Commands;

public class ResetPasswordCommandTests : IDisposable
{
    private IEssentialDbContext context;
    private readonly IPasswordSecurity passwordSecurity;

    public ResetPasswordCommandTests()
    {
        context = new ServiceCollection().AddPersistenceAsInMemory(nameof(ResetPasswordCommandTests)).BuildServiceProvider().GetService<IEssentialDbContext>();
        passwordSecurity = new PasswordSecurity();
    }
    
    public void Dispose()
    {
        context = null;
    }

    [Fact]
    public async void WhenSuccessfullOrOk()
    {
        var pasword = passwordSecurity.Hash("password");
        var user = UserDbo.Create("email@email.com", "username", pasword);
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync(default);
        
        var handler = new ForgotMyPasswordCommand.ForgotMyPasswordHandler(context, new Mock<IMediator>().Object, passwordSecurity);

        await handler.Handle(new ForgotMyPasswordCommand(
            new RequestDto<ForgotMyPasswordDtoV1>("device", 
                ForgotMyPasswordDtoV1.Create(user.Email, user.Username), null, null)), default);

        var newPass = context.Users.FirstOrDefault(x => x.Username == user.Username)?.TempPassword;
        
        Assert.NotNull(newPass);
        Assert.NotEmpty(newPass);
        Assert.NotEqual(pasword, newPass);
    }
    
    [Fact]
    public async void WhenBadRequest()
    {
        var pasword = passwordSecurity.Hash("password");
        var user = UserDbo.Create("email@email.com", "username", pasword);
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync(default);
        var handler = new ForgotMyPasswordCommand.ForgotMyPasswordHandler(context, new Mock<IMediator>().Object, passwordSecurity);

        var command = new ForgotMyPasswordCommand(
            new RequestDto<ForgotMyPasswordDtoV1>(string.Empty,
                ForgotMyPasswordDtoV1.Create("email@email.com", "username"), null, null));
        
        await Assert.ThrowsAsync<BadRequestException>(() =>
            handler.Handle(command, default));
    }

    [Theory]
    [InlineData("", "")]
    [InlineData("email@email.com", "")]
    [InlineData("", "username")]
    public async void WhenInvalidCommandParameters(string email, string username)
    {
        var handler = new ForgotMyPasswordCommand.ForgotMyPasswordHandler(context, new Mock<IMediator>().Object, passwordSecurity);
        var command = new ForgotMyPasswordCommand(
            new RequestDto<ForgotMyPasswordDtoV1>("device",
                ForgotMyPasswordDtoV1.Create(email, username), null, null));
        
        await Assert.ThrowsAsync<BadRequestException>(() =>
            handler.Handle(command, default));
    }
    
    
}