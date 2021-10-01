using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using WeeControl.App.Services.Authorization;
using WeeControl.Server.Domain.Authorization;
using WeeControl.Server.Domain.Authorization.Entities;
using WeeControl.Server.Test.Functional.Common;
using WeeControl.SharedKernel.Common.Interfaces;
using Xunit;

namespace WeeControl.Server.Test.Functional.Authorization
{
    public class RefreshCurrentTokenTests : IClassFixture<CustomWebApplicationFactory<Startup>>, IDisposable, ITestsRequireAuthentication
    {
        #region static
        public static async Task<string> GetRefreshedTokenAsync(HttpClient client, string username, string password,
            string device)
        {
            var token1 = await RequestNewTokenTests.GetNewTokenAsync(client, username, password, device);
            var token2 = string.Empty;
            
            Mock<IClientDevice> deviceMock = new();
            deviceMock.SetupAllProperties();
            deviceMock.Setup(x => x.DeviceId).Returns(device);
            deviceMock.Setup(x => x.GetTokenAsync()).ReturnsAsync(token1);
            deviceMock.Setup(x => x.SaveTokenAsync(It.IsAny<string>())).Callback<string>(y => token2 = y);
            
            IAuthenticationService service = new AuthenticationService(client, deviceMock.Object);
            var response = await service.RefreshCurrentToken();

            return token2;
        }
        #endregion
        
        private static string RandomText => new Random().NextDouble().ToString();
        
        private readonly IAuthorizationDbContext dbContext;
        private readonly CustomWebApplicationFactory<Startup> factory;
        private readonly string device;
        private Mock<IClientDevice> clientDeviceMock;

        public RefreshCurrentTokenTests(CustomWebApplicationFactory<Startup> factory)
        {
            var scope = factory.Services.GetService<IServiceScopeFactory>()?.CreateScope();
            dbContext = scope?.ServiceProvider.GetService<IAuthorizationDbContext>();
            
            this.factory = factory;
            device = nameof(RefreshCurrentTokenTests);
            
            clientDeviceMock = new Mock<IClientDevice>();
            clientDeviceMock.SetupAllProperties();
            clientDeviceMock.Setup(x => x.DeviceId).Returns(device);
            
        }

        public void Dispose()
        {
            clientDeviceMock = null;
        }

        [Fact]
        public async void WhenSendingValidRequest_HttpResponseIsSuccessCode()
        {
            var client = factory.CreateClient();
            var scope = factory.Services.GetService<IServiceScopeFactory>().CreateScope();
            var context = scope.ServiceProvider.GetService<IAuthorizationDbContext>();
            
            await context.Users.AddAsync(new User(nameof(WhenSendingValidRequest_HttpResponseIsSuccessCode), "password"));
            await context.SaveChangesAsync(default);
            
            var token = await GetRefreshedTokenAsync(client, nameof(WhenSendingValidRequest_HttpResponseIsSuccessCode), "password", nameof(RefreshCurrentTokenTests));

            Assert.NotEmpty(token);
        }
        
        [Fact]
        public async void WhenUnAuthenticatedUser_HttpResponseIsUnauthorized()
        {
            IAuthenticationService service = new AuthenticationService(factory.CreateClient(), clientDeviceMock.Object);
            var response = await service.RefreshCurrentToken();

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatuesCode);
        }

        [Fact]
        public async void WhenAuthenticatedButNotAuthorized_HttpResponseIsForbidden()
        {
            //When different device...
            var user = new User(RandomText, RandomText);
            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync(default);
            
            var token = await RequestNewTokenTests.GetNewTokenAsync(factory.CreateClient(), user.Username, user.Password, device);
            
            clientDeviceMock.Setup(x => x.DeviceId).Returns("Other Device");
            clientDeviceMock.Setup(x => x.GetTokenAsync()).ReturnsAsync(token);
            
            var service = new AuthenticationService(factory.CreateClient(), clientDeviceMock.Object);
            var response = await service.RefreshCurrentToken();

            Assert.Equal(HttpStatusCode.Forbidden, response.StatuesCode);
        }
    }
}
