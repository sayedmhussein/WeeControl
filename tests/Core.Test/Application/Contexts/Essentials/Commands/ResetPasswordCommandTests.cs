using WeeControl.Core.Application.Contexts.Essentials.Commands;
using WeeControl.Core.Application.Exceptions;
using WeeControl.Core.DomainModel.Essentials.Dto;
using WeeControl.Core.SharedKernel.DtoParent;

namespace WeeControl.Core.Test.Application.Contexts.Essentials.Commands;

public class ResetPasswordCommandTests
{
    [Fact]
    public async void WhenSuccessfulOrOk()
    {
        using var testHelper = new CoreTestHelper();
        var seed = testHelper.SeedDatabase();

        await GetHandler(testHelper).Handle(
            GetCommand(CoreTestHelper.Email, CoreTestHelper.Username, CoreTestHelper.DeviceId),
            default);

        var newPass = testHelper.EssentialDb.Users.First().TempPassword;
        Assert.NotNull(newPass);
        Assert.NotEmpty(newPass);
        Assert.NotEqual("password", newPass);
    }

    [Fact]
    public async void WhenBadRequest()
    {
        using var testHelper = new CoreTestHelper();
        testHelper.SeedDatabase();

        await Assert.ThrowsAsync<BadRequestException>(() =>
            GetHandler(testHelper).Handle(
                GetCommand(CoreTestHelper.Email, CoreTestHelper.Username, string.Empty),
                default));
    }

    [Theory]
    [InlineData("", "")]
    [InlineData("email@email.com", "")]
    [InlineData("", "username")]
    public async void WhenInvalidCommandParameters(string email, string username)
    {
        using var testHelper = new CoreTestHelper();
        var handler = GetHandler(testHelper);
        var command = GetCommand(email, username, "device");

        await Assert.ThrowsAsync<BadRequestException>(() =>
            handler.Handle(command, default));
    }

    private UserForgotMyPasswordCommand.ForgotMyPasswordHandler GetHandler(CoreTestHelper coreTestHelper)
    {
        return new UserForgotMyPasswordCommand.ForgotMyPasswordHandler(
            coreTestHelper.EssentialDb,
            coreTestHelper.MediatorMock.Object,
            coreTestHelper.PasswordSecurity);
    }

    private UserForgotMyPasswordCommand GetCommand(string email, string username, string device)
    {
        return new UserForgotMyPasswordCommand(
            RequestDto.Create(
                UserPasswordResetRequestDto.Create(email, username), device, null, null));
    }
}