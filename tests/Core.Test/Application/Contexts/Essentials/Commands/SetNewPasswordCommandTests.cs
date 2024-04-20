using WeeControl.Core.Application.Contexts.Essentials.Commands;
using WeeControl.Core.Application.Exceptions;
using WeeControl.Core.SharedKernel.DtoParent;

namespace WeeControl.Core.Test.Application.Contexts.Essentials.Commands;

public class SetNewPasswordCommandTests
{
    [Fact]
    public async void WhenRequestSentCorrect_PasswordIsChangedSuccessfully()
    {
        using var testHelper = new CoreTestHelper();
        var seed = testHelper.SeedDatabase();
        testHelper.CurrentUserInfoMock.Setup(x => x.SessionId).Returns(seed.sessionId);

        await ExecuteHandler(testHelper, CoreTestHelper.Password, "NewPassword");

        var storedPassword = testHelper.EssentialDb.Users.First(x => x.UserId == seed.userId).Password;
        Assert.Equal(testHelper.PasswordSecurity.Hash("NewPassword"), storedPassword);
    }

    [Fact]
    public async void WhenRequestSentInvalidOldPassword_ThrowNotFound()
    {
        using var testHelper = new CoreTestHelper();
        var seed = testHelper.SeedDatabase();
        testHelper.CurrentUserInfoMock.Setup(x => x.SessionId).Returns(seed.sessionId);

        var handler = new UserNewPasswordCommand.SetNewPasswordHandler(testHelper.EssentialDb,
            testHelper.CurrentUserInfoMock.Object, testHelper.PasswordSecurity);

        await Assert.ThrowsAsync<NotFoundException>(() => ExecuteHandler(testHelper, "OtherPassword", "NewPassword"));
    }

    [Theory]
    [InlineData("", "")]
    [InlineData("oldPassword", "")]
    [InlineData("", "NewPassword")]
    public async void WhenInvalidCommandParameters(string oldPassword, string newPassword)
    {
        using var testHelper = new CoreTestHelper();
        var handler = new UserNewPasswordCommand.SetNewPasswordHandler(testHelper.EssentialDb,
            testHelper.CurrentUserInfoMock.Object, testHelper.PasswordSecurity);

        await Assert.ThrowsAsync<BadRequestException>(() =>
            ExecuteHandler(testHelper, oldPassword, newPassword));
    }

    private static Task ExecuteHandler(CoreTestHelper helper, string oldPassword, string newPassword)
    {
        var requestDto = RequestDto.Create(CoreTestHelper.DeviceId, 0, 0);

        var handler = new UserNewPasswordCommand.SetNewPasswordHandler(
            helper.EssentialDb,
            helper.CurrentUserInfoMock.Object,
            helper.PasswordSecurity);

        return handler.Handle(new UserNewPasswordCommand(requestDto, oldPassword, newPassword), default);
    }
}