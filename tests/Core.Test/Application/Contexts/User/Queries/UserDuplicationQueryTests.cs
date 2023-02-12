using MediatR;
using WeeControl.Core.Application.Contexts.User.Queries;
using WeeControl.Core.Application.Exceptions;
using WeeControl.Core.DataTransferObject.Contexts.User;

namespace WeeControl.Core.Test.Application.Contexts.User.Queries;

public class UserDuplicationQueryTests
{
    [Fact]
    public async void TestForSuccess()
    {
        using var testHelper = new TestHelper();

        var handler = GetHandler(testHelper);

        Assert.Equal(Unit.Value, await handler.Handle(new UserDuplicationQuery(UserDuplicationQuery.Parameter.Username, "username1"), default));
        Assert.Equal(Unit.Value, await handler.Handle(new UserDuplicationQuery(UserDuplicationQuery.Parameter.Email, "email@email.com"), default));
        Assert.Equal(Unit.Value, await handler.Handle(new UserDuplicationQuery(UserDuplicationQuery.Parameter.Mobile, "+33"), default));
    }

    [Theory]
    [InlineData(TestHelper.Email + "x", TestHelper.Username + "y", TestHelper.MobileNo)]
    [InlineData(TestHelper.Email + "x", TestHelper.Username, TestHelper.MobileNo + "0")]
    [InlineData(TestHelper.Email, TestHelper.Username + "y", TestHelper.MobileNo + "0")]
    public async void TestsForFailures(string email, string username, string mobileNo)
    {
        using var testHelper = new TestHelper();
        
        var handler = GetHandler(testHelper);

        await Assert.ThrowsAsync<ConflictFailureException>(async () =>
        {
            await handler.Handle(
                new UserDuplicationQuery(UserDuplicationQuery.Parameter.Username, username),
                default);
            await handler.Handle(
                new UserDuplicationQuery(UserDuplicationQuery.Parameter.Email, email),
                default);
            await handler.Handle(
                new UserDuplicationQuery(UserDuplicationQuery.Parameter.Mobile, mobileNo),
                default);
        });
    }

    private UserDuplicationQuery.UserDuplicationHandler GetHandler(TestHelper testHelper)
    {
        testHelper.SeedDatabase();
        return new UserDuplicationQuery.UserDuplicationHandler(testHelper.EssentialDb);
    }
}