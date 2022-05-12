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

namespace WeeControl.test.Application.Test.EssentialContext.Commands
{
    public class LogoutEmployeeCommandTests : IDisposable
    {
        private IEssentialDbContext context;
        private RequestDto requestDto;
        private Mock<ICurrentUserInfo> currentUserInfoMock;

        public LogoutEmployeeCommandTests()
        {
            context = new ServiceCollection().AddPersistenceAsInMemory(nameof(LogoutEmployeeCommandTests)).BuildServiceProvider().GetService<IEssentialDbContext>();
            requestDto = new RequestDto("device");
            currentUserInfoMock = new Mock<ICurrentUserInfo>();
        }

        public void Dispose()
        {
            context = null;
            requestDto = null;
        }

        [Fact]
        public async void WhenSessionExistAndNotTerminated_SessionBecomeTerminated()
        {
            var session = SessionDbo.Create(Guid.NewGuid(), "device");
            await context.Sessions.AddAsync(session);
            await context.SaveChangesAsync(default);

            currentUserInfoMock.Setup(x => x.GetSessionId()).Returns(session.SessionId);

            var handler = new LogoutHandler(context, currentUserInfoMock.Object);
            var response = await handler.Handle(new LogoutCommand(requestDto), default);
            ;
            Assert.NotNull(context.Sessions.First(x => x.SessionId == session.SessionId).TerminationTs);
        }

        [Fact]
        public async void WhenRequestDtoHasDifferentSession_ThrowNotAllowedException()
        {
            var session = SessionDbo.Create(Guid.NewGuid(), "device");
            await context.Sessions.AddAsync(session);
            await context.SaveChangesAsync(default);
            currentUserInfoMock.Setup(x => x.GetSessionId()).Returns(Guid.NewGuid());

            var handler = new LogoutHandler(context, currentUserInfoMock.Object);

            await Assert.ThrowsAsync<NotAllowedException>(() =>
                handler.Handle(new LogoutCommand(requestDto), default));
        }
        
        [Fact]
        public async void WhenRequestDtoHasDifferentDevice_ThrowNotAllowedException()
        {
            var session = SessionDbo.Create(Guid.NewGuid(), "device");
            await context.Sessions.AddAsync(session);
            await context.SaveChangesAsync(default);
            requestDto.DeviceId = "Another Device ID";
            currentUserInfoMock.Setup(x => x.GetSessionId()).Returns(session.SessionId);

            var handler = new LogoutHandler(context, currentUserInfoMock.Object);

            await Assert.ThrowsAsync<NotAllowedException>(() =>
                handler.Handle(new LogoutCommand(requestDto), default));
        }
        
        [Fact]
        public async void WhenSessionAlreadyTerminated_ThrowNotAllowedException()
        {
            var session = SessionDbo.Create(Guid.NewGuid(), "device");
            session.TerminationTs = DateTime.UtcNow;
            await context.Sessions.AddAsync(session);
            await context.SaveChangesAsync(default);
            currentUserInfoMock.Setup(x => x.GetSessionId()).Returns(session.SessionId);

            var handler = new LogoutHandler(context, currentUserInfoMock.Object);

            await Assert.ThrowsAsync<NotAllowedException>(() =>
                handler.Handle(new LogoutCommand(requestDto), default));
        }
    }
}