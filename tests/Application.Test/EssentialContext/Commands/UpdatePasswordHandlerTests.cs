using System;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using WeeControl.Application.EssentialContext;
using WeeControl.Application.EssentialContext.Commands;
using WeeControl.Application.Exceptions;
using WeeControl.Application.Interfaces;
using WeeControl.Domain.Essential.Entities;
using WeeControl.Persistence;
using WeeControl.SharedKernel.Interfaces;
using WeeControl.SharedKernel.RequestsResponses;
using WeeControl.SharedKernel.Services;
using Xunit;

namespace WeeControl.Application.Test.EssentialContext.Commands;

public class UpdatePasswordHandlerTests : IDisposable
{
    private IEssentialDbContext context;
    private readonly IPasswordSecurity passwordSecurity;
    private RequestDto requestDto;
    private Mock<ICurrentUserInfo> currentUserInfoMock;

    public UpdatePasswordHandlerTests()
    {
        context = new ServiceCollection().AddPersistenceAsInMemory(nameof(UpdatePasswordHandlerTests)).BuildServiceProvider().GetService<IEssentialDbContext>();
        passwordSecurity = new PasswordSecurity();
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
        var user = UserDbo.Create(info.Email, info.Username, passwordSecurity.Hash(info.Password));
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync(default);
        
        var session = SessionDbo.Create(Guid.NewGuid(), "device");
        session.UserId = user.UserId;
        await context.Sessions.AddAsync(session);
        await context.SaveChangesAsync(default);

        currentUserInfoMock.Setup(x => x.GetSessionId()).Returns(session.SessionId);

        var handler = new UpdatePasswordHandler(context, currentUserInfoMock.Object, passwordSecurity);
        await handler.Handle(new UpdatePasswordCommand(requestDto, info.Password, "NewPassword"), default);
        
        Assert.Equal(passwordSecurity.Hash("NewPassword"), user.Password);
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

        var handler = new UpdatePasswordHandler(context, currentUserInfoMock.Object, passwordSecurity);
        
        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(new UpdatePasswordCommand(requestDto, "OtherPassword", "NewPassword"), default));
    }
}