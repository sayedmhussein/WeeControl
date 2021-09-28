using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using WeeControl.Backend.Domain.BoundedContexts.HumanResources.EmployeeModule.Entities;
using WeeControl.Backend.Persistence.BoundedContexts.HumanResources;
using WeeControl.Backend.WebApi.Test.Functional.TestHelpers;
using WeeControl.Common.SharedKernel.BoundedContexts.HumanResources.Authentication;
using WeeControl.Common.SharedKernel.BoundedContexts.HumanResources.ClientSideServices;
using WeeControl.Common.SharedKernel.Interfaces;
using Xunit;

namespace WeeControl.Backend.WebApi.Test.Functional.BoundedContexts.HumanResources.Authorization
{
    public class RequestNewTokenTests : IClassFixture<CustomWebApplicationFactory<Startup>>, IDisposable, ITestsNotRequireAuthentication
    {
        #region static
        public static async Task<string> GetNewTokenAsync(HttpClient client, string username, string password, string device)
        {
            var token = string.Empty;
            
            Mock<IClientDevice> deviceMock = new();
            deviceMock.SetupAllProperties();
            deviceMock.Setup(x => x.DeviceId).Returns(device);
            deviceMock.Setup(x => x.SaveTokenAsync(It.IsAny<string>())).Callback<string>(y => token = y);
            
            IAuthenticationService service = new AuthenticationService(client, deviceMock.Object);
            var response = await service.RequestNewToken(new RequestNewTokenDto(username, password));


            return token;
        }
        #endregion
        
        private readonly CustomWebApplicationFactory<Startup> factory;
        private readonly string device;

        public RequestNewTokenTests(CustomWebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
            device = nameof(RequestNewTokenTests);
        }

        public void Dispose()
        {
            
        }

        [Fact]
        public async void WhenSendingInvalidRequest_HttpResponseIsBadRequest()
        {
            Mock<IClientDevice> deviceMock = new();
            deviceMock.SetupAllProperties();
            deviceMock.Setup(x => x.DeviceId).Returns(device);

            IAuthenticationService service = new AuthenticationService(factory.CreateClient(), deviceMock.Object);
            var response2 = await service.RequestNewToken(null);
            
            Assert.Equal(HttpStatusCode.BadRequest, response2.StatuesCode);
        }

        [Fact]
        public async void WhenSendingValidRequest_HttpResponseIsSuccessCode()
        {
            var scope = factory.Services.GetService<IServiceScopeFactory>().CreateScope();
            var context = scope.ServiceProvider.GetService<HumanResourcesDbContext>();

            var username = new Random().NextDouble().ToString();
            await context.Employees.AddAsync(Employee.Create("Code", "FirstName", "LastName", username, "password"));
            await context.SaveChangesAsync(default);
            
            
            var token = await GetNewTokenAsync(factory.CreateClient(), username, "password", typeof(RequestNewTokenTests).Namespace);
            
            Assert.NotEmpty(token);
        }
    }
}