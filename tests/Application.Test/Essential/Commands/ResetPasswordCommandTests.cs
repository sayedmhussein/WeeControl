using System.Linq;
using WeeControl.Core.Application.Contexts.User.Commands;
using WeeControl.Core.Application.Exceptions;
using WeeControl.Core.DataTransferObject.BodyObjects;
using WeeControl.Core.DataTransferObject.Contexts.User;
using Xunit;

namespace WeeControl.ApiApp.Application.Test.Essential.Commands;

public class ResetPasswordCommandTests
{
    [Fact]
    public async void WhenSuccessfulOrOk()
    {
        using var testHelper = new TestHelper();
        var seed = testHelper.SeedDatabase();

        await GetHandler(testHelper).Handle(
            GetCommand(TestHelper.Email, TestHelper.Username, TestHelper.DeviceId),
            default);

        var newPass = testHelper.EssentialDb.Users.First().TempPassword;
        Assert.NotNull(newPass);
        Assert.NotEmpty(newPass);
        Assert.NotEqual("password", newPass);
    }

    [Fact]
    public async void WhenBadRequest()
    {
        using var testHelper = new TestHelper();
        testHelper.SeedDatabase();
        
        await Assert.ThrowsAsync<BadRequestException>(() =>
            GetHandler(testHelper).Handle(
                GetCommand(TestHelper.Email, TestHelper.Username, string.Empty),
                default));
    }

    [Theory]
    [InlineData("", "")]
    [InlineData("email@email.com", "")]
    [InlineData("", "username")]
    public async void WhenInvalidCommandParameters(string email, string username)
    {
        using var testHelper = new TestHelper();
        var handler = GetHandler(testHelper);
        var command = GetCommand(email, username, "device");

        await Assert.ThrowsAsync<BadRequestException>(() =>
            handler.Handle(command, default));
    }

    private UserForgotMyPasswordCommand.ForgotMyPasswordHandler GetHandler(TestHelper testHelper)
    {
        return new UserForgotMyPasswordCommand.ForgotMyPasswordHandler(
            testHelper.EssentialDb,
            testHelper.MediatorMock.Object,
            testHelper.PasswordSecurity);
    }

    private UserForgotMyPasswordCommand GetCommand(string email, string username, string device)
    {
        return new UserForgotMyPasswordCommand(
            RequestDto.Create(
                UserPasswordResetRequestDto.Create(email, username), device, null, null));
    }
}