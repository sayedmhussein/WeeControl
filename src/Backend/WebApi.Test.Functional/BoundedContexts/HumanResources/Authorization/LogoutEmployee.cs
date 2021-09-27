using System;
using System.Net;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using WeeControl.Backend.Domain.BoundedContexts.HumanResources;
using WeeControl.Backend.Domain.BoundedContexts.HumanResources.EmployeeModule.Entities;
using WeeControl.Backend.WebApi.Test.Functional.TestHelpers;
using WeeControl.Common.SharedKernel.BoundedContexts.HumanResources.ClientSideServices;
using WeeControl.Common.SharedKernel.Interfaces;
using Xunit;

namespace WeeControl.Backend.WebApi.Test.Functional.BoundedContexts.HumanResources.Authorization
{
    public class LogoutEmployee : IClassFixture<CustomWebApplicationFactory<Startup>>, IDisposable, ITestsRequireAuthentication
    {
        private readonly CustomWebApplicationFactory<Startup> factory;
        private readonly IHumanResourcesDbContext dbContext;
        private readonly string device;
        private Mock<IClientDevice> clientDeviceMock;
        
        public LogoutEmployee(CustomWebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
            var scope = factory.Services.GetService<IServiceScopeFactory>().CreateScope();
            dbContext = scope.ServiceProvider.GetService<IHumanResourcesDbContext>();
            
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
            var username = new Random().NextDouble().ToString();
            await dbContext.Employees.AddAsync(Employee.Create("Code", "FirstName", "LastName", username, "password"));
            await dbContext.SaveChangesAsync(default);
            
            var token = await RefreshCurrentTokenTests.GetRefreshedTokenAsync(factory.CreateClient(), nameof(WhenSendingValidRequest_HttpResponseIsSuccessCode), "password", device);
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
            await dbContext.Employees.AddAsync(Employee.Create("Code", "FirstName", "LastName", username, "password"));
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