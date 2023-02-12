using System;
using System.Threading.Tasks;
using WeeControl.Core.Application.Contexts.User.Commands;
using WeeControl.Core.Application.Exceptions;
using WeeControl.Core.DataTransferObject.BodyObjects;
using WeeControl.Core.Domain.Contexts.User;
using Xunit;

namespace WeeControl.ApiApp.Application.Test.Essential.Commands;

public class SetNewPasswordCommandTests
{
    private readonly RequestDto requestDto = RequestDto.Create("device", 0, 0);

    [Fact]
    public async void WhenRequestSentCorrect_PasswordIsChangedSuccessfully()
    {
        using var testHelper = new TestHelper();
        var seed = testHelper.SeedDatabase();
        testHelper.CurrentUserInfoMock.Setup(x => x.SessionId).Returns(seed.sessionId);

        await ExecuteHandler(testHelper, TestHelper.Password, "NewPassword");
        Assert.Equal(testHelper.PasswordSecurity.Hash("NewPassword"), seed.User.Password);
    }

    [Fact]
    public async void WhenRequestSentInvalidOldPassword_ThrowNotFound()
    {
        using var testHelper = new TestHelper();
        var user = testHelper.GetUserDboWithEncryptedPassword("username", "password");
        await testHelper.EssentialDb.Users.AddAsync(user);
        await testHelper.EssentialDb.SaveChangesAsync(default);

        var session = UserSessionDbo.Create(Guid.NewGuid(), "device", "0000");
        session.UserId = user.UserId;
        await testHelper.EssentialDb.UserSessions.AddAsync(session);
        await testHelper.EssentialDb.SaveChangesAsync(default);

        testHelper.CurrentUserInfoMock.Setup(x => x.SessionId).Returns(session.SessionId);

        var handler = new UserNewPasswordCommand.SetNewPasswordHandler(testHelper.EssentialDb, testHelper.CurrentUserInfoMock.Object, testHelper.PasswordSecurity);

        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(new UserNewPasswordCommand(requestDto, "OtherPassword", "NewPassword"), default));
    }

    [Theory]
    [InlineData("", "")]
    [InlineData("oldPassword", "")]
    [InlineData("", "NewPassword")]
    public async void WhenInvalidCommandParameters(string oldPassword, string newPassword)
    {
        using var testHelper = new TestHelper();
        var handler = new UserNewPasswordCommand.SetNewPasswordHandler(testHelper.EssentialDb, testHelper.CurrentUserInfoMock.Object, testHelper.PasswordSecurity);

        await Assert.ThrowsAsync<BadRequestException>(() =>
            handler.Handle(new UserNewPasswordCommand(requestDto, oldPassword, newPassword), default));
    }

    private Task ExecuteHandler(TestHelper helper, string oldPassword, string newPassword)
    {
        var handler = new UserNewPasswordCommand.SetNewPasswordHandler(
            helper.EssentialDb, 
            helper.CurrentUserInfoMock.Object, 
            helper.PasswordSecurity);
        
        return handler.Handle(new UserNewPasswordCommand(requestDto, oldPassword, newPassword), default);
    }
}