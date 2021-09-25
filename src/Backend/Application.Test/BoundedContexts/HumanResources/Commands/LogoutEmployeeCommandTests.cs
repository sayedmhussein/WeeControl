using System;
using System.Linq;
using System.Net;
using Microsoft.Extensions.DependencyInjection;
using WeeControl.Backend.Application.BoundContexts.HumanResources.Commands.LogoutEmployee;
using WeeControl.Backend.Application.Exceptions;
using WeeControl.Backend.Domain.BoundedContexts.HumanResources;
using WeeControl.Backend.Domain.BoundedContexts.HumanResources.EmployeeModule.Entities;
using WeeControl.Backend.Persistence;
using Xunit;

namespace WeeControl.Backend.Application.Test.BoundedContexts.HumanResources.Commands
{
    public class LogoutEmployeeCommandTests : IDisposable
    {
        private IHumanResourcesDbContext context;
        
        public LogoutEmployeeCommandTests()
        {
            context = new ServiceCollection().AddPersistenceAsInMemory(nameof(LogoutEmployeeCommandTests)).BuildServiceProvider().GetService<IHumanResourcesDbContext>();
        }

        public void Dispose()
        {
            context = null;
        }

        [Fact]
        public async void WhenSessionExistAndNotTerminated_ResponseIsOkandSessionBecomeTerminated()
        {
            var session = Session.Create(Guid.NewGuid(), "device");
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
            var session = Session.Create(Guid.NewGuid(), "device");
            await context.Sessions.AddAsync(session);
            await context.SaveChangesAsync(default);

            var handler = new LogoutEmployeeHandler(context);

            await Assert.ThrowsAsync<NotAllowedException>(() =>
                handler.Handle(new LogoutEmployeeCommand("device", Guid.NewGuid()), default));
        }
        
        [Fact]
        public async void WhenRequestDtoHasDifferentDevice_ThrowNotAllowedException()
        {
            var session = Session.Create(Guid.NewGuid(), "device");
            await context.Sessions.AddAsync(session);
            await context.SaveChangesAsync(default);

            var handler = new LogoutEmployeeHandler(context);

            await Assert.ThrowsAsync<NotAllowedException>(() =>
                handler.Handle(new LogoutEmployeeCommand("different device", session.SessionId), default));
        }
        
        [Fact]
        public async void WhenSessionAlreadyTerminated_ThrowNotAllowedException()
        {
            var session = Session.Create(Guid.NewGuid(), "device");
            session.TerminationTs = DateTime.UtcNow;
            await context.Sessions.AddAsync(session);
            await context.SaveChangesAsync(default);

            var handler = new LogoutEmployeeHandler(context);

            await Assert.ThrowsAsync<NotAllowedException>(() =>
                handler.Handle(new LogoutEmployeeCommand("device", session.SessionId), default));
        }
    }
}