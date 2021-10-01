using System;
using System.Net;
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
    public class LogoutEmployee : IClassFixture<CustomWebApplicationFactory<Startup>>, IDisposable, ITestsRequireAuthentication
    {
        private static string RandomText => new Random().NextDouble().ToString();
        
        private readonly CustomWebApplicationFactory<Startup> factory;
        private readonly IAuthorizationDbContext dbContext;
        private readonly string device;
        private Mock<IClientDevice> clientDeviceMock;
        
        public LogoutEmployee(CustomWebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
            var scope = factory.Services.GetService<IServiceScopeFactory>()?.CreateScope();
            dbContext = scope?.ServiceProvider.GetService<IAuthorizationDbContext>();
            
            device = nameof(LogoutEmployee);
            
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
            var employee = new User(RandomText, RandomText);
            await dbContext.Users.AddAsync(employee);
            await dbContext.SaveChangesAsync(default);
            
            var token = await RefreshCurrentTokenTests.GetRefreshedTokenAsync(factory.CreateClient(), employee.Username, employee.Password, device);
            clientDeviceMock.Setup(x => x.GetTokenAsync()).ReturnsAsync(token);
            var service = new AuthenticationService(factory.CreateClient(), clientDeviceMock.Object);

            var response = await service.Logout();
            
            Assert.Equal(HttpStatusCode.OK, response.StatuesCode);
        }

        [Fact]
        public async void WhenUnAuthenticatedUser_HttpResponseIsUnauthorized()
        {
            var service = new AuthenticationService(factory.CreateClient(), clientDeviceMock.Object);

            var response = await service.Logout();
            
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatuesCode);
        }

        [Fact]
        public async void WhenAuthenticatedButNotAuthorized_HttpResponseIsForbidden()
        {
            var username = new Random().NextDouble().ToString();
            await dbContext.Users.AddAsync(new User(username, "password"));
            await dbContext.SaveChangesAsync(default);

            var token = await RefreshCurrentTokenTests.GetRefreshedTokenAsync(factory.CreateClient(), username, "password", device);
            clientDeviceMock.Setup(x => x.GetTokenAsync()).ReturnsAsync(token);
            var service = new AuthenticationService(factory.CreateClient(), clientDeviceMock.Object);

            var response1 = await service.Logout();
            Assert.Equal(HttpStatusCode.OK, response1.StatuesCode);
            
            var response2 = await service.Logout();
            Assert.Equal(HttpStatusCode.Forbidden, response2.StatuesCode);
        }
    }
}