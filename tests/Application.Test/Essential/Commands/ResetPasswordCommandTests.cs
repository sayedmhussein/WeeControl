using System;
using System.Linq;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using WeeControl.Application.Essential;
using WeeControl.Application.Essential.Commands;
using WeeControl.Application.Exceptions;
using WeeControl.Domain.Essential.Entities;
using WeeControl.Persistence;
using WeeControl.SharedKernel.DataTransferObjects.User;
using WeeControl.SharedKernel.Interfaces;
using WeeControl.SharedKernel.RequestsResponses;
using WeeControl.SharedKernel.Services;
using Xunit;

namespace WeeControl.Application.Test.Essential.Commands;

public class ResetPasswordCommandTests
{
    [Fact]
    public async void WhenSuccessfullOrOk()
    {
        using var testHelper = new TestHelper();
        var pasword = testHelper.PasswordSecurity.Hash("password");
        var user = UserDbo.Create("email@email.com", "username", pasword);
        await testHelper.EssentialDb.Users.AddAsync(user);
        await testHelper.EssentialDb.SaveChangesAsync(default);
        
        var handler = new ForgotMyPasswordCommand.ForgotMyPasswordHandler(testHelper.EssentialDb, new Mock<IMediator>().Object, testHelper.PasswordSecurity);

        await handler.Handle(new ForgotMyPasswordCommand(
            new RequestDto<ForgotMyPasswordDtoV1>("device", 
                ForgotMyPasswordDtoV1.Create(user.Email, user.Username), null, null)), default);

        var newPass = testHelper.EssentialDb.Users.FirstOrDefault(x => x.Username == user.Username)?.TempPassword;
        
        Assert.NotNull(newPass);
        Assert.NotEmpty(newPass);
        Assert.NotEqual(pasword, newPass);
    }
    
    [Fact]
    public async void WhenBadRequest()
    {
        using var testHelper = new TestHelper();
        var pasword = testHelper.PasswordSecurity.Hash("password");
        var user = UserDbo.Create("email@email.com", "username", pasword);
        await testHelper.EssentialDb.Users.AddAsync(user);
        await testHelper.EssentialDb.SaveChangesAsync(default);
        var handler = new ForgotMyPasswordCommand.ForgotMyPasswordHandler(testHelper.EssentialDb, new Mock<IMediator>().Object, testHelper.PasswordSecurity);

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
        using var testHelper = new TestHelper();
        var handler = new ForgotMyPasswordCommand.ForgotMyPasswordHandler(testHelper.EssentialDb, new Mock<IMediator>().Object, testHelper.PasswordSecurity);
        var command = new ForgotMyPasswordCommand(
            new RequestDto<ForgotMyPasswordDtoV1>("device",
                ForgotMyPasswordDtoV1.Create(email, username), null, null));
        
        await Assert.ThrowsAsync<BadRequestException>(() =>
            handler.Handle(command, default));
    }
}