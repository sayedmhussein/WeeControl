using System;
using System.Linq;
using System.Net;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using WeeControl.Backend.Application.BoundContexts.HumanResources.Queries.GetNewToken;
using WeeControl.Backend.Application.Exceptions;
using WeeControl.Backend.Domain.BoundedContexts.HumanResources;
using WeeControl.Backend.Domain.BoundedContexts.HumanResources.EmployeeModule.Entities;
using WeeControl.Backend.Persistence;
using WeeControl.Common.SharedKernel.BoundedContexts.HumanResources.Authentication;
using WeeControl.Common.SharedKernel.BoundedContexts.Shared;
using WeeControl.Common.SharedKernel.Interfaces;
using WeeControl.Common.UserSecurityLib.Interfaces;
using WeeControl.Common.UserSecurityLib.Services;
using Xunit;

namespace WeeControl.Backend.Application.Test.BoundedContexts.HumanResources.Queries
{
    public class GetNewTokenTests : IDisposable
    {
        private readonly IJwtService jwtService;
        
        private IHumanResourcesDbContext context;
        private Mock<Mediator> mediatRMock;
        private Mock<IConfiguration> configurationMock;
        
        public GetNewTokenTests()
        {
            context = new ServiceCollection().AddPersistenceAsInMemory(nameof(GetNewTokenTests)).BuildServiceProvider().GetService<IHumanResourcesDbContext>();
            jwtService = new JwtService();
            
            configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(x => x["Jwt:Key"]).Returns(new string('a', 30));
        }

        public void Dispose()
        {
            context = null;
            mediatRMock = null;
            configurationMock = null;
        }

        #region using username and password
        [Fact]
        public async void WhenValidUsernameAndPassword_ReturnProperDto()
        {
            var query = new GetNewTokenQuery(new RequestDto<RequestNewTokenDto>(nameof(WhenValidUsernameAndPassword_ReturnProperDto), new RequestNewTokenDto("admin", "admin")));
            
            var response = await new GetNewTokenHandler(context, jwtService, null, configurationMock.Object).Handle(query, default);
            
            Assert.Equal(HttpStatusCode.OK, response.StatuesCode);
            Assert.NotEmpty(response.Payload.Token);
            Assert.NotEmpty(response.Payload.FullName);
        }
        
        [Fact]
        public async void WhenValidUsernameAndPasswordButExistingSessionIsActive_ShouldNotCreatAnotherSession()
        {
            var service = new GetNewTokenHandler(context, jwtService, null, configurationMock.Object);
            
            var query1 = new GetNewTokenQuery(new RequestDto<RequestNewTokenDto>("device", new RequestNewTokenDto("admin", "admin")));
            await service.Handle(query1, default);
            var count1 = await context.Sessions.CountAsync();

            var query2 = new GetNewTokenQuery(new RequestDto("device"), context.Sessions.First().SessionId);
            await service.Handle(query2, default);
            var count2 = await context.Sessions.CountAsync();
            
            Assert.Equal(count1, count2);
        }
        
        public async void WhenValidUsernameAndPasswordButExistingSessionIsActive_ShouldCreatAnotherSession()
        {
            var service = new GetNewTokenHandler(context, jwtService, null, configurationMock.Object);
            
            var query1 = new GetNewTokenQuery(new RequestDto<RequestNewTokenDto>("device", new RequestNewTokenDto("admin", "admin")));
            await service.Handle(query1, default);
            var count1 = await context.Sessions.CountAsync();

            var query2 = new GetNewTokenQuery(new RequestDto("another device"), context.Sessions.First().SessionId);
            await service.Handle(query2, default);
            var count2 = await context.Sessions.CountAsync();
            
            Assert.Equal(count1 + 1, count2);
        }
        
        [Fact]
        public async void WhenUsernameAndPasswordNotMatched_ThrowsNotFoundException()
        {
            var query = new GetNewTokenQuery(new RequestDto<RequestNewTokenDto>(nameof(WhenUsernameAndPasswordNotMatched_ThrowsNotFoundException),new RequestNewTokenDto("unmatched", "unmatched")));
            
            var service = new GetNewTokenHandler(context, jwtService, null, configurationMock.Object);
            
            await Assert.ThrowsAsync<NotFoundException>(() => service.Handle(query, default));
            
        }

        [Theory]
        [InlineData("", "", "")]
        [InlineData("device", "", "")]
        [InlineData("device", "username", "")]
        [InlineData("", "username", "")]
        [InlineData("", "", "password")]
        public async void WhenUsernameAndPasswordAndDeviceNotProper_ThrowBadRequestException(string device, string username, string password)
        {
            var query = new GetNewTokenQuery(new RequestDto<RequestNewTokenDto>(device,new RequestNewTokenDto(username, password)));
            
            var service = new GetNewTokenHandler(context, jwtService, null, configurationMock.Object);
            
            await Assert.ThrowsAsync<BadRequestException>(() => service.Handle(query, default));
        }
        #endregion

        #region using session-id and device-id
        [Fact]
        public async void WhenExistingSessionIsValid_ReturnProperResponse()
        {
            var request = new RequestDto("device");

            var session = Session.Create(Guid.NewGuid(), "device");
            session.Employee = context.Employees.First();
            await context.Sessions.AddAsync(session , default);
            await context.SaveChangesAsync(default);
            
            var query = new GetNewTokenQuery(request, session.SessionId);
            
            var response = await new GetNewTokenHandler(context, jwtService, null, configurationMock.Object).Handle(query, default);
            
            Assert.Equal(HttpStatusCode.OK, response.StatuesCode);
            Assert.NotEmpty(response.Payload.Token);
            Assert.NotEmpty(response.Payload.FullName);
        }
        
        [Fact]
        public async void WhenExistingSessionIsTerminated_ThrowNotAllowedException()
        {
            var request = new RequestDto("device");

            var session = Session.Create(Guid.NewGuid(), "device");
            session.Employee = context.Employees.First();
            session.TerminationTs = DateTime.UtcNow;
            await context.Sessions.AddAsync(session , default);
            await context.SaveChangesAsync(default);
            
            var query = new GetNewTokenQuery(request, session.SessionId);
            
            var service = new GetNewTokenHandler(context, jwtService, null, configurationMock.Object);
            
            await Assert.ThrowsAsync<NotAllowedException>(() => service.Handle(query, default));
        }
        #endregion

       
    }
}