using Microsoft.EntityFrameworkCore;
using WeeControl.Core.Application.Contexts.Essentials.Queries;
using WeeControl.Core.Application.Exceptions;

namespace WeeControl.Core.Test.Application.Contexts.Essentials.Queries;

public class GetUserIdAndSessionVerificationQueryTests
{
    [Fact]
    public async void WhenUserExist_ReturnUserId()
    {
        using var helper = new CoreTestHelper();
        var seed = helper.SeedDatabase();
        helper.CurrentUserInfoMock.Setup(x => x.SessionId)
            .Returns(seed.sessionId);

        var handler =
            new GetUserIdAndSessionVerificationQuery.UserIdVerificationHandler(helper.EssentialDb,
                helper.CurrentUserInfoMock.Object);

        Assert.Equal(seed.personId, await handler.Handle(new GetUserIdAndSessionVerificationQuery(), default));
    }

    [Fact]
    public async void WhenNoSession_ThrowNotAllowedException()
    {
        using var helper = new CoreTestHelper();
        var seed = helper.SeedDatabase();

        var handler =
            new GetUserIdAndSessionVerificationQuery.UserIdVerificationHandler(helper.EssentialDb,
                helper.CurrentUserInfoMock.Object);

        await Assert.ThrowsAsync<NotAllowedException>(() =>
            handler.Handle(new GetUserIdAndSessionVerificationQuery(), default));
    }

    [Fact]
    public async void WhenSessionIsTerminated_ThrowNotAllowedException()
    {
        using var helper = new CoreTestHelper();
        var seed = helper.SeedDatabase();
        helper.CurrentUserInfoMock.Setup(x => x.SessionId)
            .Returns(seed.sessionId);
        var session = await helper.EssentialDb.UserSessions.FirstAsync(x => x.SessionId == seed.sessionId);
        session.TerminationTs = DateTime.UtcNow;
        await helper.EssentialDb.SaveChangesAsync(default);

        var handler =
            new GetUserIdAndSessionVerificationQuery.UserIdVerificationHandler(helper.EssentialDb,
                helper.CurrentUserInfoMock.Object);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            handler.Handle(new GetUserIdAndSessionVerificationQuery(), default));
    }

    [Fact]
    public async void WhenUserAccountIsLocked_ThrowNotAllowedException()
    {
        using var helper = new CoreTestHelper();
        var seed = helper.SeedDatabase();
        helper.CurrentUserInfoMock.Setup(x => x.SessionId)
            .Returns(seed.sessionId);
        var user = await helper.EssentialDb.Person.FirstAsync(x => x.PersonId == seed.personId);
        user.Suspend("Test");
        await helper.EssentialDb.SaveChangesAsync(default);

        var handler =
            new GetUserIdAndSessionVerificationQuery.UserIdVerificationHandler(helper.EssentialDb,
                helper.CurrentUserInfoMock.Object);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            handler.Handle(new GetUserIdAndSessionVerificationQuery(), default));
    }
}