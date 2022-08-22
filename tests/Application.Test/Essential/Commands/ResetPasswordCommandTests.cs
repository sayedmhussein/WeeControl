using System.Linq;
using WeeControl.ApiApp.Application.Contexts.Essential.Commands;
using WeeControl.ApiApp.Application.Exceptions;
using WeeControl.Common.SharedKernel.Contexts.Essential.DataTransferObjects.User;
using WeeControl.Common.SharedKernel.RequestsResponses;
using Xunit;

namespace WeeControl.ApiApp.Application.Test.Essential.Commands;

public class ResetPasswordCommandTests
{
    [Fact]
    public async void WhenSuccessfulOrOk()
    {
        using var testHelper = new TestHelper();
        var user = testHelper.GetUserDboWithEncryptedPassword("username", "password");
        await testHelper.EssentialDb.Users.AddAsync(user);
        await testHelper.EssentialDb.SaveChangesAsync(default);
        
        await GetHandler(testHelper).Handle(GetCommand(user.Email, user.Username, "device"), 
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
        var user = testHelper.GetUserDboWithEncryptedPassword("username", "password");
        await testHelper.EssentialDb.Users.AddAsync(user);
        await testHelper.EssentialDb.SaveChangesAsync(default);
        var handler = GetHandler(testHelper);

        var command = GetCommand(user.Email, user.Username, string.Empty);
        
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