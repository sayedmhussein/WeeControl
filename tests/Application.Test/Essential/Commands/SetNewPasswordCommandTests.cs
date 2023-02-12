using System;
using System.Linq;
using System.Threading.Tasks;
using WeeControl.Core.Application.Contexts.User.Commands;
using WeeControl.Core.Application.Exceptions;
using WeeControl.Core.DataTransferObject.BodyObjects;
using WeeControl.Core.Domain.Contexts.User;
using Xunit;

namespace WeeControl.ApiApp.Application.Test.Essential.Commands;

public class SetNewPasswordCommandTests
{
    [Fact]
    public async void WhenRequestSentCorrect_PasswordIsChangedSuccessfully()
    {
        using var testHelper = new TestHelper();
        var seed = testHelper.SeedDatabase();
        testHelper.CurrentUserInfoMock.Setup(x => x.SessionId).Returns(seed.sessionId);

        await ExecuteHandler(testHelper, TestHelper.Password, "NewPassword");

        var storedPassword = testHelper.EssentialDb.Users.First(x => x.UserId == seed.userId).Password;
        Assert.Equal(testHelper.PasswordSecurity.Hash("NewPassword"), storedPassword);
    }

    [Fact]
    public async void WhenRequestSentInvalidOldPassword_ThrowNotFound()
    {
        using var testHelper = new TestHelper();
        var seed = testHelper.SeedDatabase();
        testHelper.CurrentUserInfoMock.Setup(x => x.SessionId).Returns(seed.sessionId);

        var handler = new UserNewPasswordCommand.SetNewPasswordHandler(testHelper.EssentialDb, testHelper.CurrentUserInfoMock.Object, testHelper.PasswordSecurity);

        await Assert.ThrowsAsync<NotFoundException>(() => ExecuteHandler(testHelper, "OtherPassword", "NewPassword"));
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
            ExecuteHandler(testHelper, oldPassword, newPassword));
    }

    private Task ExecuteHandler(TestHelper helper, string oldPassword, string newPassword)
    {
        var requestDto = RequestDto.Create(TestHelper.DeviceId, 0, 0);
        
        var handler = new UserNewPasswordCommand.SetNewPasswordHandler(
            helper.EssentialDb, 
            helper.CurrentUserInfoMock.Object, 
            helper.PasswordSecurity);
        
        return handler.Handle(new UserNewPasswordCommand(requestDto, oldPassword, newPassword), default);
    }
}