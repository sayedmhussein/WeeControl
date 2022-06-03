using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Moq;
using WeeControl.Application.Essential.Queries;
using WeeControl.Domain.Essential.Entities;
using Xunit;

namespace WeeControl.Application.Test.Essential.Queries;

public class GetListOfUsersTests 
{
    [Fact]
    public async void Success()
    {
        using var testHelper = new TestHelper();
        var user = UserDbo.Create("email@email.com", "username", "password", "TRR");
        await testHelper.EssentialDb.Users.AddAsync(user, default);
        await testHelper.EssentialDb.Users.AddAsync(UserDbo.Create("email@email.com1", "username1", "password", "TRR"), default);
        await testHelper.EssentialDb.Users.AddAsync(UserDbo.Create("email@email.com2", "username2", "password", "TRR"), default);
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
        var user = UserDbo.Create("email@email.com", "username", "password", "TRR1");
        await testHelper.EssentialDb.Users.AddAsync(user, default);
        await testHelper.EssentialDb.Users.AddAsync(UserDbo.Create("email@email.com1", "username1", "password", "TRR2"), default);
        await testHelper.EssentialDb.Users.AddAsync(UserDbo.Create("email@email.com2", "username2", "password", "TRR3"), default);
        await testHelper.EssentialDb.SaveChangesAsync(default);
        var session = SessionDbo.Create(user.UserId, nameof(GetListOfUsersTests));
        await testHelper.EssentialDb.Sessions.AddAsync(session);
        await testHelper.EssentialDb.SaveChangesAsync(default);
        testHelper.CurrentUserInfoMock.Setup(x => x.GetSessionId()).Returns(session.SessionId);
        testHelper.CurrentUserInfoMock.Setup(x => x.GetTerritoriesListAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<string>() {user.TerritoryId, "TRR2"});

        var handler = await new GetListOfUsersQuery.GetListOfUsersHandler(testHelper.EssentialDb, testHelper.CurrentUserInfoMock.Object).Handle(new GetListOfUsersQuery(),default);
        
        Assert.Equal(2, handler.Payload.Count());
    }
}