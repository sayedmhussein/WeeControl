using System;
using System.Linq;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using WeeControl.Backend.Application.EssentialContext.Queries;
using WeeControl.Backend.Application.Exceptions;
using WeeControl.Backend.Application.Interfaces;
using WeeControl.Backend.Domain.Databases.Essential;
using WeeControl.Backend.Domain.Databases.Essential.DatabaseObjects.EssentialsObjects;
using WeeControl.Backend.Persistence;
using WeeControl.Common.SharedKernel.DataTransferObjects.Essential.User;
using WeeControl.Common.SharedKernel.RequestsResponses;
using WeeControl.Common.UserSecurityLib.Interfaces;
using WeeControl.Common.UserSecurityLib.Services;
using Xunit;

namespace WeeControl.test.Application.Test.EssentialContext.Queries
{
    public class GetNewTokenHandlerTests : IDisposable
    {
        private const string Email = "email@provider.com";
        private const string Username = "username";
        private const string Password = "password";
        
        private readonly IJwtService jwtService;
        
        private IEssentialDbContext context;
        private Mock<IMediator> mediatRMock;
        private Mock<IConfiguration> configurationMock;
        private Mock<ICurrentUserInfo> currentUserInfoMock;
        
        public GetNewTokenHandlerTests()
        {
            context = new ServiceCollection().AddPersistenceAsInMemory().BuildServiceProvider().GetService<IEssentialDbContext>();
            context.Users.Add(UserDbo.Create(Email, Username, Password));
            context.SaveChanges();

            jwtService = new JwtService();

            mediatRMock = new Mock<IMediator>();
            
            configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(x => x["Jwt:Key"]).Returns(new string('a', 30));

            currentUserInfoMock = new Mock<ICurrentUserInfo>();
        }

        public void Dispose()
        {
            context = null;
            mediatRMock = null;
            configurationMock = null;
        }

        #region using username and password
        [Fact]
        public async void WhenValidUsernameAndPassword_ReturnToken()
        {
            var dto = new RequestDto<LoginDto>(nameof(WhenValidUsernameAndPassword_ReturnToken), new LoginDto(Username, Password));
            var query = new GetNewTokenQuery(dto);

            var service = new GetNewTokenHandler(context, jwtService, mediatRMock.Object, configurationMock.Object, currentUserInfoMock.Object);
            var response = await service.Handle(query, default);
            
            Assert.NotEmpty(response.Payload.Token);
            Assert.NotEmpty(response.Payload.FullName);
        }
        
        [Fact]
        public async void WhenValidUsernameAndPasswordButExistingSessionIsActive_ShouldNotCreatAnotherSession()
        {
            var service = new GetNewTokenHandler(context, jwtService, mediatRMock.Object, configurationMock.Object, currentUserInfoMock.Object);
            
            var query = new GetNewTokenQuery(
                new RequestDto<LoginDto>(nameof(WhenValidUsernameAndPasswordButExistingSessionIsActive_ShouldNotCreatAnotherSession), 
                    new LoginDto(Username, Password)));
            await service.Handle(query, default);
            var count1 = await context.Sessions.CountAsync();
            
            await service.Handle(query, default);
            var count2 = await context.Sessions.CountAsync();
            
            Assert.Equal(count1, count2);
        }
        
        [Fact]
        public async void WhenValidUsernameAndPasswordButExistingSessionIsNotActive_ShouldCreatAnotherSession()
        {
            var service = new GetNewTokenHandler(context, jwtService, mediatRMock.Object, configurationMock.Object, currentUserInfoMock.Object);
            
            var query = new GetNewTokenQuery(new RequestDto<LoginDto>(
                nameof(WhenValidUsernameAndPasswordButExistingSessionIsNotActive_ShouldCreatAnotherSession), 
                new LoginDto(Username, Password)));
            await service.Handle(query, default);
            var count1 = await context.Sessions.CountAsync();
            
            context.Sessions.First().TerminationTs = DateTime.UtcNow;
            await context.SaveChangesAsync(default);
            
            await service.Handle(query, default);
            var count2 = await context.Sessions.CountAsync();
            
            Assert.Equal(count1 + 1, count2);
        }
        
        [Fact]
        public async void WhenUsernameAndPasswordNotMatched_ThrowsNotFoundException()
        {
            var query = new GetNewTokenQuery(new RequestDto<LoginDto>(
                nameof(WhenUsernameAndPasswordNotMatched_ThrowsNotFoundException),
                new LoginDto("unmatched", "unmatched")));
            
            var service = new GetNewTokenHandler(context, jwtService, mediatRMock.Object, configurationMock.Object, currentUserInfoMock.Object);
            
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
            var query = new GetNewTokenQuery(new RequestDto<LoginDto>(
                device,
                new LoginDto(username, password)));
            
            var service = new GetNewTokenHandler(context, jwtService, mediatRMock.Object, configurationMock.Object, currentUserInfoMock.Object);
            
            await Assert.ThrowsAsync<BadRequestException>(() => service.Handle(query, default));
        }
        #endregion

        #region using session-id and device-id
        [Fact]
        public async void WhenExistingSessionIsValid_ReturnToken()
        {
            var request = new RequestDto("device");

            var session = SessionDbo.Create(Guid.NewGuid(), "device");
            session.User = context.Users.First();
            await context.Sessions.AddAsync(session , default);
            await context.SaveChangesAsync(default);

            currentUserInfoMock.Setup(x => x.GetSessionId()).Returns(session.SessionId);
            
            var query = new GetNewTokenQuery(request);
            
            var response = await new GetNewTokenHandler(context, jwtService, mediatRMock.Object, configurationMock.Object, currentUserInfoMock.Object).Handle(query, default);
            
            Assert.NotEmpty(response.Payload.Token);
            Assert.NotEmpty(response.Payload.FullName);
        }
        
        [Fact]
        public async void WhenExistingSessionIsTerminated_ThrowNotAllowedException()
        {
            var request = new RequestDto("device");

            var session = SessionDbo.Create(Guid.NewGuid(), "device");
            session.User = context.Users.First();
            session.TerminationTs = DateTime.UtcNow;
            await context.Sessions.AddAsync(session , default);
            await context.SaveChangesAsync(default);
            currentUserInfoMock.Setup(x => x.GetSessionId()).Returns(session.SessionId);
            
            var query = new GetNewTokenQuery(request);
            
            var service = new GetNewTokenHandler(context, jwtService, mediatRMock.Object, configurationMock.Object, currentUserInfoMock.Object);
            
            await Assert.ThrowsAsync<NotAllowedException>(() => service.Handle(query, default));
        }

        [Fact]
        public async void WhenUsingDifferentDevice_ThrowNotAllowedException()
        {
            var request = new RequestDto("other device");

            var session = SessionDbo.Create(Guid.NewGuid(), "device");
            session.User = context.Users.First();
            await context.Sessions.AddAsync(session , default);
            await context.SaveChangesAsync(default);
            
            currentUserInfoMock.Setup(x => x.GetSessionId()).Returns(session.SessionId);
            
            var query = new GetNewTokenQuery(request);
            var service = new GetNewTokenHandler(context, jwtService, mediatRMock.Object, configurationMock.Object, currentUserInfoMock.Object);
            
            await Assert.ThrowsAsync<NotAllowedException>(() => service.Handle(query, default));
        }
        #endregion
    }
}