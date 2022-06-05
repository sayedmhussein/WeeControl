using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Moq;
using WeeControl.Application.Essential.Queries;
using WeeControl.Domain.Contexts.Essential;
using Xunit;

namespace WeeControl.Application.Test.Essential.Queries;

public class GetListOfUsersTests 
{
    [Fact]
    public async void Success()
    {
        using var testHelper = new TestHelper();
        var user = testHelper.GetUserDbo("username", "password");
        await testHelper.EssentialDb.Users.AddAsync(user, default);
        await testHelper.EssentialDb.Users.AddAsync(testHelper.GetUserDbo("username2", "password"), default);
        await testHelper.EssentialDb.Users.AddAsync(testHelper.GetUserDbo("username3", "password"), default);
        await testHelper.EssentialDb.SaveChangesAsync(default);
        var session = SessionDbo.Create(user.UserId, nameof(GetListOfUsersTests));
        await testHelper.EssentialDb.Sessions.AddAsync(session);
        await testHelper.EssentialDb.SaveChangesAsync(default);
        testHelper.CurrentUserInfoMock.Setup(x => x.GetSessionId()).Returns(session.SessionId);
        testHelper.CurrentUserInfoMock.Setup(x => x.GetTerritoriesListAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<string>() {user.TerritoryId});

        var handler = await new GetListOfUsersQuery.GetListOfUsersHandler(testHelper.EssentialDb, testHelper.CurrentUserInfoMock.Object).Handle(new GetListOfUsersQuery(),default);
        
        Assert.Equal(3, handler.Payload.Count());
    }
    
    [Fact]
    public async void WhenNotFullList()
    {
        using var testHelper = new TestHelper();
        var user = testHelper.GetUserDbo("username", "password", "TR1");
        await testHelper.EssentialDb.Users.AddAsync(user, default);
        await testHelper.EssentialDb.Users.AddAsync(testHelper.GetUserDbo("username", "password", "TR2"), default);
        await testHelper.EssentialDb.Users.AddAsync(testHelper.GetUserDbo("username", "password", "TR3"), default);
        await testHelper.EssentialDb.SaveChangesAsync(default);
        var session = SessionDbo.Create(user.UserId, nameof(GetListOfUsersTests));
        await testHelper.EssentialDb.Sessions.AddAsync(session);
        await testHelper.EssentialDb.SaveChangesAsync(default);
        testHelper.CurrentUserInfoMock.Setup(x => x.GetSessionId()).Returns(session.SessionId);
        testHelper.CurrentUserInfoMock.Setup(x => x.GetTerritoriesListAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<string>() {user.TerritoryId, "TR1"});

        var handler = await new GetListOfUsersQuery.GetListOfUsersHandler(testHelper.EssentialDb, testHelper.CurrentUserInfoMock.Object).Handle(new GetListOfUsersQuery(),default);
        
        Assert.Single(handler.Payload);
    }
}