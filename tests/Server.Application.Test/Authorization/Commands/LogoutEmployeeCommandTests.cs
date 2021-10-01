using System;
using System.Net;
using Moq;
using WeeControl.Application.Authentication.Commands.LogoutEmployee;
using WeeControl.Application.Common.Exceptions;
using WeeControl.Server.Domain.Authorization.Entities;
using WeeControl.Server.Domain.HumanResources;
using Xunit;

namespace WeeControl.Server.Application.Test.Authorization.Commands
{
    public class LogoutEmployeeCommandTests : IDisposable
    {
        private IHumanResourcesDbContext context;
        
        public LogoutEmployeeCommandTests()
        {
            context = new Mock<IHumanResourcesDbContext>().Object;
            //context = new ServiceCollection().AddPersistenceAsInMemory(nameof(LogoutEmployeeCommandTests)).BuildServiceProvider().GetService<IHumanResourcesDbContext>();
        }

        public void Dispose()
        {
            context = null;
        }

        [Fact]
        public async void WhenSessionExistAndNotTerminated_ResponseIsOkandSessionBecomeTerminated()
        {
            var session = UserSession.Create(Guid.NewGuid(), "device");
            await context.Sessions.AddAsync(session);
            await context.SaveChangesAsync(default);

            var handler = new LogoutEmployeeHandler(context);
            var response = await handler.Handle(new LogoutEmployeeCommand("device", session.SessionId), default);

            Assert.Equal(HttpStatusCode.OK, response.StatuesCode);
            Assert.NotNull(context.Sessions.First(x => x.SessionId == session.SessionId).TerminationTs);
        }

        [Fact]
        public async void WhenRequestDtoHasDifferentSession_ThrowNotAllowedException()
        {
            var session = UserSession.Create(Guid.NewGuid(), "device");
            await context.Sessions.AddAsync(session);
            await context.SaveChangesAsync(default);

            var handler = new LogoutEmployeeHandler(context);

            await Assert.ThrowsAsync<NotAllowedException>(() =>
                handler.Handle(new LogoutEmployeeCommand("device", Guid.NewGuid()), default));
        }
        
        [Fact]
        public async void WhenRequestDtoHasDifferentDevice_ThrowNotAllowedException()
        {
            var session = UserSession.Create(Guid.NewGuid(), "device");
            await context.Sessions.AddAsync(session);
            await context.SaveChangesAsync(default);

            var handler = new LogoutEmployeeHandler(context);

            await Assert.ThrowsAsync<NotAllowedException>(() =>
                handler.Handle(new LogoutEmployeeCommand("different device", session.SessionId), default));
        }
        
        [Fact]
        public async void WhenSessionAlreadyTerminated_ThrowNotAllowedException()
        {
            var session = UserSession.Create(Guid.NewGuid(), "device");
            session.TerminationTs = DateTime.UtcNow;
            await context.Sessions.AddAsync(session);
            await context.SaveChangesAsync(default);

            var handler = new LogoutEmployeeHandler(context);

            await Assert.ThrowsAsync<NotAllowedException>(() =>
                handler.Handle(new LogoutEmployeeCommand("device", session.SessionId), default));
        }
    }
}