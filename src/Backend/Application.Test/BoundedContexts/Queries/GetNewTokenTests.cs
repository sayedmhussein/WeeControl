using System;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using WeeControl.Backend.Application.BoundContexts.HumanResources.Queries.GetNewToken;
using WeeControl.Backend.Domain.BoundedContexts.HumanResources;
using WeeControl.Backend.Domain.BoundedContexts.HumanResources.EmployeeModule.Entities;
using WeeControl.Backend.Persistence;
using WeeControl.Common.SharedKernel.Interfaces;
using WeeControl.Common.UserSecurityLib.Interfaces;
using WeeControl.Common.UserSecurityLib.Services;
using Xunit;

namespace WeeControl.Backend.Application.Test.BoundedContexts.Queries
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

        [Fact]
        public async void WhenValidUsernameAndPassword_ReturnTokenString()
        {
            // var query = new GetNewTokenQuery(new RequestDto<RequestNewTokenDto>(new RequestNewTokenDto("admin", "admin"), nameof(WhenValidUsernameAndPassword_ReturnTokenString)));
            //
            // var token = await new GetNewTokenHandler(context, jwtService, null, configurationMock.Object).Handle(query, default);
            //
            // Assert.NotEmpty(token.Payload.Token);
        }
        
        [Fact]
        public async void WhenUsernameAndPasswordNotMatched_ThrowsNotFoundException()
        {
            // var query = new GetNewTokenQuery(new RequestDto<RequestNewTokenDto>(new RequestNewTokenDto("unmatched", "unmatched"), nameof(WhenValidUsernameAndPassword_ReturnTokenString)));
            //
            // var service = new GetNewTokenHandler(context, jwtService, null, configurationMock.Object);
            //
            // await Assert.ThrowsAsync<NotFoundException>(() => service.Handle(query, default));
        }

        [Fact]
        public async void WhenExistingSessionIdIsSupplied_ReturnTokenString()
        {
            var requestMock = new Mock<IRequestDto>();
            requestMock.Setup(x => x.DeviceId).Returns(nameof(WhenExistingSessionIdIsSupplied_ReturnTokenString));

            var sessionId = Guid.NewGuid();
            await context.Sessions.AddAsync( Session.Create(Guid.NewGuid(), ""));
            await context.SaveChangesAsync(default);
            
            var query = new GetNewTokenQuery(requestMock.Object, Guid.Empty);
        }
    }
}