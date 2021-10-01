using System;
using System.Linq;
using System.Net;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using WeeControl.Application.Authentication.Queries.GetNewToken;
using WeeControl.Application.Common.Exceptions;
using WeeControl.Server.Domain.Authorization;
using WeeControl.Server.Domain.Authorization.Entities;
using WeeControl.Server.Persistence;
using WeeControl.SharedKernel.Authorization.DtosV1;
using WeeControl.SharedKernel.Common.DtosV1;
using WeeControl.SharedKernel.Common.Interfaces;
using WeeControl.SharedKernel.Security.Services;
using Xunit;

namespace WeeControl.Server.Application.Test.Authorization.Queries
{
    public class GetNewTokenTests : IDisposable
    {
        private static string RandomString => new Random().NextDouble().ToString();
        private const string Username = "username";
        private const string Password = "password";
        
        private readonly IJwtService jwtService;
        
        private IAuthorizationDbContext context;
        private Mock<Mediator> mediatRMock;
        private Mock<IConfiguration> configurationMock;
        
        public GetNewTokenTests()
        {
            context = new ServiceCollection().AddPersistenceAsInMemoryDb().BuildServiceProvider().GetService<IAuthorizationDbContext>();
            context.Users.Add(new User(Username, Password));
            context.SaveChanges();

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
            var user = new User(RandomString, RandomString);
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync(default);
            
            var query = new GetNewTokenQuery(
                new RequestDto<RequestNewTokenDto>(nameof(WhenValidUsernameAndPassword_ReturnProperDto), 
                new RequestNewTokenDto(user.Username, user.Password)));

            var service = new GetNewTokenHandler(context, jwtService, null, configurationMock.Object);
            var response = await service.Handle(query, default);
            
            Assert.Equal(HttpStatusCode.OK, response.StatuesCode);
            Assert.NotEmpty(response.Payload.Token);
            Assert.NotEmpty(response.Payload.FullName);
        }
        
        [Fact]
        public async void WhenValidUsernameAndPasswordButExistingSessionIsActive_ShouldNotCreatAnotherSession()
        {
            var service = new GetNewTokenHandler(context, jwtService, null, configurationMock.Object);
            
            var query = new GetNewTokenQuery(
                new RequestDto<RequestNewTokenDto>(nameof(WhenValidUsernameAndPasswordButExistingSessionIsActive_ShouldNotCreatAnotherSession), 
                    new RequestNewTokenDto(Username, Password)));
            await service.Handle(query, default);
            var count1 = await context.Sessions.CountAsync();
            
            await service.Handle(query, default);
            var count2 = await context.Sessions.CountAsync();
            
            Assert.Equal(count1, count2);
        }
        
        [Fact]
        public async void WhenValidUsernameAndPasswordButExistingSessionIsNotActive_ShouldCreatAnotherSession()
        {
            var user = new User(RandomString, RandomString);
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync(default);
            
            var service = new GetNewTokenHandler(context, jwtService, null, configurationMock.Object);
            
            var query = new GetNewTokenQuery(new RequestDto<RequestNewTokenDto>(
                nameof(WhenValidUsernameAndPasswordButExistingSessionIsNotActive_ShouldCreatAnotherSession), 
                new RequestNewTokenDto(user.Username, user.Password)));
            
            await service.Handle(query, default);
            var count1 = await context.Sessions.CountAsync();
            
            context.Sessions.First(x => x.User.Username == user.Username).TerminationTs = DateTime.UtcNow;
            await context.SaveChangesAsync(default);
            
            await service.Handle(query, default);
            var count2 = await context.Sessions.CountAsync();
            
            Assert.Equal(count1 + 1, count2);
        }
        
        [Fact]
        public async void WhenUsernameAndPasswordNotMatched_ThrowsNotFoundException()
        {
            var query = new GetNewTokenQuery(new RequestDto<RequestNewTokenDto>(
                nameof(WhenUsernameAndPasswordNotMatched_ThrowsNotFoundException),
                new RequestNewTokenDto("unmatched", "unmatched")));
            
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
            var query = new GetNewTokenQuery(new RequestDto<RequestNewTokenDto>(
                device,
                new RequestNewTokenDto(username, password)));
            
            var service = new GetNewTokenHandler(context, jwtService, null, configurationMock.Object);
            
            await Assert.ThrowsAsync<BadRequestException>(() => service.Handle(query, default));
        }
        #endregion

        #region using session-id and device-id
        [Fact]
        public async void WhenExistingSessionIsValid_ReturnProperResponse()
        {
            var request = new RequestDto("device");

            var session = new UserSession(context.Users.First().UserId, "device");
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

            var session = new UserSession(Guid.NewGuid(), "device");
            session.User = context.Users.First();
            session.TerminationTs = DateTime.UtcNow;
            await context.Sessions.AddAsync(session , default);
            await context.SaveChangesAsync(default);
            
            var query = new GetNewTokenQuery(request, session.SessionId);
            
            var service = new GetNewTokenHandler(context, jwtService, null, configurationMock.Object);
            
            await Assert.ThrowsAsync<NotAllowedException>(() => service.Handle(query, default));
        }

        public async void WhenUsingDifferentDevice_ThrowNotAllowedException()
        {
            var request = new RequestDto("other device");

            var session = new UserSession(Guid.NewGuid(), "device");
            session.User = context.Users.First();
            await context.Sessions.AddAsync(session , default);
            await context.SaveChangesAsync(default);
            
            var query = new GetNewTokenQuery(request, session.SessionId);
            var service = new GetNewTokenHandler(context, jwtService, null, configurationMock.Object);
            
            await Assert.ThrowsAsync<NotAllowedException>(() => service.Handle(query, default));
        }
        #endregion
    }
}