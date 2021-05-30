using System;
using Xunit;
using Moq;
using MySystem.Application.Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using MySystem.Persistence;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using MySystem.Domain.EntityDbo.EmployeeSchema;
using MySystem.SharedKernel.Definition;
using Application.Employee.Command.RefreshEmployeeToken.V1;

namespace MySystem.Application.Test.Employee.Command.RefreshEmployeeToken.V1
{
    public class RefreshEmployeeTokenHandlerTests : IDisposable
    {
        private IMySystemDbContext dbContext;

        public RefreshEmployeeTokenHandlerTests()
        {
            dbContext = new ServiceCollection().AddPersistenceAsInMemory(null).BuildServiceProvider().GetService<IMySystemDbContext>();
        }

        public void Dispose()
        {
            dbContext = null;
        }

        [Fact]
        public async void WhenValidSession_ReturnNewToken()
        {
            var employee = await dbContext.Employees.FirstOrDefaultAsync(x => x.Username == "username");
            var session = new EmployeeSessionDbo() { Employee = employee, DeviceId = "DeviceId" };
            await dbContext.EmployeeSessions.AddAsync(session);
            await dbContext.SaveChangesAsync(default);
            //
            var jwtMock = new Mock<IJwtService>();
            jwtMock.Setup(x => x.GetClaims(It.IsAny<string>())).Returns(new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>() {new Claim(UserClaim.Session, session.Id.ToString()) })));
            jwtMock.Setup(x => x.GenerateJwtToken(It.IsAny<IEnumerable<Claim>>(), It.IsAny<string>(), It.IsAny<DateTime>())).Returns("Token");

            var command = new RefreshEmployeeTokenCommand() { Token = "bla" };
            var token = await new RefreshEmployeeTokenHandler(dbContext, jwtMock.Object).Handle(command, default);

            Assert.NotEmpty(token.Payload);
        }
    }
}
