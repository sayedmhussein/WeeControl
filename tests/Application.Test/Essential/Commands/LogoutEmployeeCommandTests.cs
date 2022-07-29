using System;
using System.Linq;
using WeeControl.Application.Contexts.Essential.Commands;
using WeeControl.Application.Exceptions;
using WeeControl.Domain.Contexts.Essential;
using WeeControl.SharedKernel.RequestsResponses;
using Xunit;

namespace WeeControl.Application.Test.Essential.Commands;

public class LogoutEmployeeCommand1Tests
{
    [Fact]
    public async void WhenSessionExistAndNotTerminated_SessionBecomeTerminated()
    {
        using var testHelper = new TestHelper();
        var session = SessionDbo.Create(Guid.NewGuid(), "device");
        await testHelper.EssentialDb.UserSessions.AddAsync(session);
        await testHelper.EssentialDb.SaveChangesAsync(default);
        //
        testHelper.CurrentUserInfoMock.Setup(x => x.SessionId).Returns(session.SessionId);
        //
        var handler = new UserLogoutCommand.LogoutHandler(testHelper.EssentialDb, testHelper.CurrentUserInfoMock.Object);

        var dto = RequestDto.Create("device", null, null);
        await handler.Handle(new UserLogoutCommand(dto), default);
        
        Assert.NotNull(testHelper.EssentialDb.UserSessions.First(x => x.SessionId == session.SessionId).TerminationTs);
    }

    [Fact]
    public async void WhenRequestDtoHasDifferentSession_ThrowNotAllowedException()
    {
        using var testHelper = new TestHelper();
        var session = SessionDbo.Create(Guid.NewGuid(), "device");
        await testHelper.EssentialDb.UserSessions.AddAsync(session);
        await testHelper.EssentialDb.SaveChangesAsync(default);
        testHelper.CurrentUserInfoMock.Setup(x => x.SessionId).Returns(Guid.NewGuid());
    
        var handler = new UserLogoutCommand.LogoutHandler(testHelper.EssentialDb, testHelper.CurrentUserInfoMock.Object);
    
        await Assert.ThrowsAsync<NotAllowedException>(() =>
            handler.Handle(new UserLogoutCommand(RequestDto.Create("device", 0,0)), default));
    }
        
    [Fact]
    public async void WhenRequestDtoHasDifferentDevice_ThrowNotAllowedException()
    {
        using var testHelper = new TestHelper();
        var session = SessionDbo.Create(Guid.NewGuid(), "device");
        await testHelper.EssentialDb.UserSessions.AddAsync(session);
        await testHelper.EssentialDb.SaveChangesAsync(default);
        testHelper.CurrentUserInfoMock.Setup(x => x.SessionId).Returns(session.SessionId);
    
        var handler = new UserLogoutCommand.LogoutHandler(testHelper.EssentialDb, testHelper.CurrentUserInfoMock.Object);
    
        await Assert.ThrowsAsync<NotAllowedException>(() =>
            handler.Handle(new UserLogoutCommand(RequestDto.Create("Another device", 0, 0)), default));
    }
        
    [Fact]
    public async void WhenSessionAlreadyTerminated_ThrowNotAllowedException()
    {
        using var testHelper = new TestHelper();
        var session = SessionDbo.Create(Guid.NewGuid(), "device");
        session.TerminationTs = DateTime.UtcNow;
        await testHelper.EssentialDb.UserSessions.AddAsync(session);
        await testHelper.EssentialDb.SaveChangesAsync(default);
        testHelper.CurrentUserInfoMock.Setup(x => x.SessionId).Returns(session.SessionId);
    
        var handler = new UserLogoutCommand.LogoutHandler(testHelper.EssentialDb, testHelper.CurrentUserInfoMock.Object);
    
        await Assert.ThrowsAsync<NotAllowedException>(() =>
            handler.Handle(new UserLogoutCommand(RequestDto.Create("device", 0, 0)), default));
    }
    
    [Fact]
    public async void WhenDeviceIDNotSupplied_ThrowBadRequestException()
    {
        using var testHelper = new TestHelper();
        var session = SessionDbo.Create(Guid.NewGuid(), "device");
        await testHelper.EssentialDb.UserSessions.AddAsync(session);
        await testHelper.EssentialDb.SaveChangesAsync(default);
        //
        testHelper.CurrentUserInfoMock.Setup(x => x.SessionId).Returns(session.SessionId);
        //
        var handler = new UserLogoutCommand.LogoutHandler(testHelper.EssentialDb, testHelper.CurrentUserInfoMock.Object);
        
        await Assert.ThrowsAsync<NotAllowedException>(() =>
            handler.Handle(new UserLogoutCommand(RequestDto.Create(string.Empty, 0, 0)), default));
    }
    
    [Fact]
    public async void WhenSessionIDIsNull_ThrowArgumentNullException()
    {
        using var testHelper = new TestHelper();
        var session = SessionDbo.Create(Guid.NewGuid(), "device");
        await testHelper.EssentialDb.UserSessions.AddAsync(session);
        await testHelper.EssentialDb.SaveChangesAsync(default);
        //
        testHelper.CurrentUserInfoMock.Setup(x => x.SessionId).Returns((Guid?) null);
        //
        var handler = new UserLogoutCommand.LogoutHandler(testHelper.EssentialDb, testHelper.CurrentUserInfoMock.Object);
        
        await Assert.ThrowsAsync<ArgumentNullException>(() =>
            handler.Handle(new UserLogoutCommand(RequestDto.Create("device", 0, 0)), default));
    }
}