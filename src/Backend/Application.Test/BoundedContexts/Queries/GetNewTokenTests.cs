using Microsoft.Extensions.DependencyInjection;
using WeeControl.Backend.Application.BoundContexts.HumanResources.Queries.GetNewToken;
using WeeControl.Backend.Domain.BoundedContexts.HumanResources;
using WeeControl.Backend.Persistence;
using WeeControl.Common.SharedKernel.BoundedContextDtos.HumanResources.Authorization;
using WeeControl.Common.SharedKernel.Obsolute.Common;
using WeeControl.Common.UserSecurityLib.Interfaces;
using WeeControl.Common.UserSecurityLib.Services;
using Xunit;

namespace WeeControl.Backend.Application.Test.BoundedContexts.Queries
{
    public class GetNewTokenTests
    {
        private IHumanResourcesDbContext context;
        private IJwtService jwtService;
        
        public GetNewTokenTests()
        {
            context = new ServiceCollection().AddPersistenceAsInMemory(nameof(GetNewTokenTests)).BuildServiceProvider().GetService<IHumanResourcesDbContext>();
            jwtService = new JwtService();
        }

        [Fact]
        public async void WhenValidUsernameAndPassword_ReturnTokenString()
        {
            var query = new GetNewTokenQuery(new RequestDto<RequestNewTokenDto>()
            {
                DeviceId = nameof(WhenValidUsernameAndPassword_ReturnTokenString),
                Payload = new RequestNewTokenDto()
                {
                    Username = "admin", Password = "admin"
                }
            });

            var token = await new GetNewTokenHandler(context, jwtService, null).Handle(query, default);
            
            Assert.NotEmpty(token.Payload.Token);
        }
    }
}