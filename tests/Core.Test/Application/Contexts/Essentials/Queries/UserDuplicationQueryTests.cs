using MediatR;
using WeeControl.Core.Application.Contexts.Essentials.Queries;
using WeeControl.Core.Application.Exceptions;

namespace WeeControl.Core.Test.Application.Contexts.Essentials.Queries;

public class UserDuplicationQueryTests
{
    [Fact]
    public async void TestForSuccess()
    {
        using var testHelper = new CoreTestHelper();

        var handler = GetHandler(testHelper);

        Assert.Equal(Unit.Value, await handler.Handle(new UserDuplicationQuery(UserDuplicationQuery.Parameter.Username, "username1"), default));
        Assert.Equal(Unit.Value, await handler.Handle(new UserDuplicationQuery(UserDuplicationQuery.Parameter.Email, "email@email.com"), default));
        Assert.Equal(Unit.Value, await handler.Handle(new UserDuplicationQuery(UserDuplicationQuery.Parameter.Mobile, "+33"), default));
    }

    [Theory]
    [InlineData(CoreTestHelper.Email + "x", CoreTestHelper.Username + "y", CoreTestHelper.MobileNo)]
    [InlineData(CoreTestHelper.Email + "x", CoreTestHelper.Username, CoreTestHelper.MobileNo + "0")]
    [InlineData(CoreTestHelper.Email, CoreTestHelper.Username + "y", CoreTestHelper.MobileNo + "0")]
    public async void TestsForFailures(string email, string username, string mobileNo)
    {
        using var testHelper = new CoreTestHelper();
        
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

    private UserDuplicationQuery.UserDuplicationHandler GetHandler(CoreTestHelper coreTestHelper)
    {
        coreTestHelper.SeedDatabase();
        return new UserDuplicationQuery.UserDuplicationHandler(coreTestHelper.EssentialDb);
    }
}