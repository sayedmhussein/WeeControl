using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using WeeControl.Backend.Application.EssentialContext.Commands;
using WeeControl.Backend.Application.Exceptions;
using WeeControl.Backend.Application.Interfaces;
using WeeControl.Backend.Domain.Databases.Essential;
using WeeControl.Backend.Domain.Databases.Essential.DatabaseObjects.EssentialsObjects;
using WeeControl.Backend.Persistence;
using WeeControl.Common.SharedKernel.RequestsResponses;
using Xunit;

namespace WeeControl.test.Application.Test.EssentialContext.Commands;

public class UpdatePasswordHandlerTests : IDisposable
{
    private IEssentialDbContext context;
    private RequestDto requestDto;
    private Mock<ICurrentUserInfo> currentUserInfoMock;

    public UpdatePasswordHandlerTests()
    {
        context = new ServiceCollection().AddPersistenceAsInMemory(nameof(UpdatePasswordHandlerTests)).BuildServiceProvider().GetService<IEssentialDbContext>();
        requestDto = new RequestDto("device");
        currentUserInfoMock = new Mock<ICurrentUserInfo>();
    }

    public void Dispose()
    {
        context = null;
        requestDto = null;
    }

    [Fact]
    public async void WhenRequestSentCorrect_PasswordIsChangedSuccessfully()
    {
        var info = (Email: "email@email.com", Username: nameof(UpdatePasswordHandlerTests), Password: "password");
        var user = UserDbo.Create(info.Email, info.Username, info.Password);
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync(default);
        
        var session = SessionDbo.Create(Guid.NewGuid(), "device");
        session.UserId = user.UserId;
        await context.Sessions.AddAsync(session);
        await context.SaveChangesAsync(default);

        currentUserInfoMock.Setup(x => x.GetSessionId()).Returns(session.SessionId);

        var handler = new UpdatePasswordHandler(context, currentUserInfoMock.Object);
        await handler.Handle(new UpdatePasswordCommand(requestDto, info.Password, "NewPassword"), default);
        
        Assert.Equal("NewPassword", user.Password);
    }
    
    [Fact]
    public async void WhenRequestSentInvalidOldPassword_ThrowNotFound()
    {
        var info = (Email: "email@email.com", Username: nameof(UpdatePasswordHandlerTests), Password: "password");
        var user = UserDbo.Create(info.Email, info.Username, info.Password);
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync(default);
        
        var session = SessionDbo.Create(Guid.NewGuid(), "device");
        session.UserId = user.UserId;
        await context.Sessions.AddAsync(session);
        await context.SaveChangesAsync(default);

        currentUserInfoMock.Setup(x => x.GetSessionId()).Returns(session.SessionId);

        var handler = new UpdatePasswordHandler(context, currentUserInfoMock.Object);
        
        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(new UpdatePasswordCommand(requestDto, "OtherPassword", "NewPassword"), default));
    }
}