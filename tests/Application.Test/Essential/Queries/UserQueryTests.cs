using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Moq;
using WeeControl.Application.Contexts.Essential.Queries;
using WeeControl.Domain.Contexts.Essential;
using Xunit;

namespace WeeControl.Application.Test.Essential.Queries;

public class UserQueryTests 
{
    [Fact]
    public async void Success()
    {
        using var testHelper = new TestHelper();
        var user = testHelper.GetUserDboWithEncryptedPassword("username", "password");
        await testHelper.EssentialDb.Users.AddAsync(user, default);
        await testHelper.EssentialDb.Users.AddAsync(testHelper.GetUserDboWithEncryptedPassword("username2", "password"), default);
        await testHelper.EssentialDb.Users.AddAsync(testHelper.GetUserDboWithEncryptedPassword("username3", "password"), default);
        await testHelper.EssentialDb.SaveChangesAsync(default);
        var session = SessionDbo.Create(user.UserId, nameof(UserQueryTests));
        await testHelper.EssentialDb.UserSessions.AddAsync(session);
        await testHelper.EssentialDb.SaveChangesAsync(default);
        testHelper.CurrentUserInfoMock.Setup(x => x.SessionId).Returns(session.SessionId);
        testHelper.CurrentUserInfoMock.Setup(x => x.GetTerritoriesListAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<string>() {user.TerritoryId});

        var handler = await new UserQuery.UserHandler(testHelper.EssentialDb, testHelper.CurrentUserInfoMock.Object, testHelper.MediatorMock.Object).Handle(new UserQuery(),default);
        
        Assert.Equal(3, handler.Payload.Count());
    }
    
    [Fact]
    public async void WhenNotFullList()
    {
        using var testHelper = new TestHelper();
        var user = testHelper.GetUserDboWithEncryptedPassword("username", "password", "TR1");
        await testHelper.EssentialDb.Users.AddAsync(user, default);
        await testHelper.EssentialDb.Users.AddAsync(testHelper.GetUserDboWithEncryptedPassword("username", "password", "TR2"), default);
        await testHelper.EssentialDb.Users.AddAsync(testHelper.GetUserDboWithEncryptedPassword("username", "password", "TR3"), default);
        await testHelper.EssentialDb.SaveChangesAsync(default);
        var session = SessionDbo.Create(user.UserId, nameof(UserQueryTests));
        await testHelper.EssentialDb.UserSessions.AddAsync(session);
        await testHelper.EssentialDb.SaveChangesAsync(default);
        testHelper.CurrentUserInfoMock.Setup(x => x.SessionId).Returns(session.SessionId);
        testHelper.CurrentUserInfoMock.Setup(x => x.GetTerritoriesListAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<string>() {user.TerritoryId, "TR1"});

        var handler = await new UserQuery.UserHandler(testHelper.EssentialDb, testHelper.CurrentUserInfoMock.Object, testHelper.MediatorMock.Object).Handle(new UserQuery(),default);
        
        Assert.Single(handler.Payload);
    }
}