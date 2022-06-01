using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using WeeControl.Application.Essential;
using WeeControl.Application.Essential.Queries;
using WeeControl.Application.Interfaces;
using WeeControl.Application.Test.EssentialContext.Commands;
using WeeControl.Domain.Essential.Entities;
using WeeControl.Persistence;
using WeeControl.SharedKernel.RequestsResponses;
using Xunit;

namespace WeeControl.Application.Test.EssentialContext.Queries;

public class GetListOfUsersTests : IDisposable
{
    private IEssentialDbContext context;
    private RequestDto requestDto;
    private Mock<ICurrentUserInfo> currentUserInfoMock;
    
    public GetListOfUsersTests()
    {
        context = new ServiceCollection().AddPersistenceAsInMemory(nameof(GetListOfUsersTests)).BuildServiceProvider().GetService<IEssentialDbContext>();
        requestDto = new RequestDto("device");
        currentUserInfoMock = new Mock<ICurrentUserInfo>();
    }
    
    public void Dispose()
    {
        context = null;
        requestDto = null;
    }

    [Fact]
    public async void Success()
    {
        var user = UserDbo.Create("email@email.com", "username", "password", "TRR");
        await context.Users.AddAsync(user, default);
        await context.Users.AddAsync(UserDbo.Create("email@email.com1", "username1", "password", "TRR"), default);
        await context.Users.AddAsync(UserDbo.Create("email@email.com2", "username2", "password", "TRR"), default);
        await context.SaveChangesAsync(default);
        var session = SessionDbo.Create(user.UserId, nameof(GetListOfUsersTests));
        await context.Sessions.AddAsync(session);
        await context.SaveChangesAsync(default);
        currentUserInfoMock.Setup(x => x.GetSessionId()).Returns(session.SessionId);
        currentUserInfoMock.Setup(x => x.GetTerritoriesListAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<string>() {user.TerritoryId});

        var handler = await new GetListOfUsersQuery.GetListOfUsersHandler(context, currentUserInfoMock.Object).Handle(new GetListOfUsersQuery(),default);
        
        Assert.Equal(3, handler.Payload.Count());
    }
    
    [Fact]
    public async void WhenNotFullList()
    {
        var user = UserDbo.Create("email@email.com", "username", "password", "TRR1");
        await context.Users.AddAsync(user, default);
        await context.Users.AddAsync(UserDbo.Create("email@email.com1", "username1", "password", "TRR2"), default);
        await context.Users.AddAsync(UserDbo.Create("email@email.com2", "username2", "password", "TRR3"), default);
        await context.SaveChangesAsync(default);
        var session = SessionDbo.Create(user.UserId, nameof(GetListOfUsersTests));
        await context.Sessions.AddAsync(session);
        await context.SaveChangesAsync(default);
        currentUserInfoMock.Setup(x => x.GetSessionId()).Returns(session.SessionId);
        currentUserInfoMock.Setup(x => x.GetTerritoriesListAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<string>() {user.TerritoryId, "TRR2"});

        var handler = await new GetListOfUsersQuery.GetListOfUsersHandler(context, currentUserInfoMock.Object).Handle(new GetListOfUsersQuery(),default);
        
        Assert.Equal(2, handler.Payload.Count());
    }
}