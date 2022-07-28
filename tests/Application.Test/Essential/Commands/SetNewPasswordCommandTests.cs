using System;
using WeeControl.Application.Contexts.Essential.Commands;
using WeeControl.Application.Exceptions;
using WeeControl.Domain.Contexts.Essential;
using WeeControl.SharedKernel.Interfaces;
using WeeControl.SharedKernel.RequestsResponses;
using Xunit;

namespace WeeControl.Application.Test.Essential.Commands;

public class SetNewPasswordCommandTests
{
    private readonly IRequestDto requestDto = RequestDto.Create("device", 0, 0);

    [Fact]
    public async void WhenRequestSentCorrect_PasswordIsChangedSuccessfully()
    {
        using var testHelper = new TestHelper();
        var user = testHelper.GetUserDboWithEncryptedPassword("username", "password");
        await testHelper.EssentialDb.Users.AddAsync(user);
        await testHelper.EssentialDb.SaveChangesAsync(default);
        
        var session = SessionDbo.Create(Guid.NewGuid(), "device");
        session.UserId = user.UserId;
        await testHelper.EssentialDb.UserSessions.AddAsync(session);
        await testHelper.EssentialDb.SaveChangesAsync(default);

        testHelper.CurrentUserInfoMock.Setup(x => x.GetSessionId()).Returns(session.SessionId);

        var handler = new SetNewPasswordCommand.SetNewPasswordHandler(testHelper.EssentialDb, testHelper.CurrentUserInfoMock.Object, testHelper.PasswordSecurity);
        await handler.Handle(new SetNewPasswordCommand(requestDto, "password", "NewPassword"), default);
        
        Assert.Equal(testHelper.PasswordSecurity.Hash("NewPassword"), user.Password);
    }
    
    [Fact]
    public async void WhenRequestSentInvalidOldPassword_ThrowNotFound()
    {
        using var testHelper = new TestHelper();
        var user = testHelper.GetUserDboWithEncryptedPassword("username", "password");
        await testHelper.EssentialDb.Users.AddAsync(user);
        await testHelper.EssentialDb.SaveChangesAsync(default);
        
        var session = SessionDbo.Create(Guid.NewGuid(), "device");
        session.UserId = user.UserId;
        await testHelper.EssentialDb.UserSessions.AddAsync(session);
        await testHelper.EssentialDb.SaveChangesAsync(default);

        testHelper.CurrentUserInfoMock.Setup(x => x.GetSessionId()).Returns(session.SessionId);

        var handler = new SetNewPasswordCommand.SetNewPasswordHandler(testHelper.EssentialDb, testHelper.CurrentUserInfoMock.Object, testHelper.PasswordSecurity);
        
        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(new SetNewPasswordCommand(requestDto, "OtherPassword", "NewPassword"), default));
    }
    
    [Theory]
    [InlineData("", "")]
    [InlineData("oldPassword", "")]
    [InlineData("", "NewPassword")]
    public async void WhenInvalidCommandParameters(string oldPassword, string newPassword)
    {
        using var testHelper = new TestHelper();
        var handler = new SetNewPasswordCommand.SetNewPasswordHandler(testHelper.EssentialDb, testHelper.CurrentUserInfoMock.Object, testHelper.PasswordSecurity);

        await Assert.ThrowsAsync<BadRequestException>(() =>
            handler.Handle(new SetNewPasswordCommand(requestDto, oldPassword, newPassword), default));
    }
}