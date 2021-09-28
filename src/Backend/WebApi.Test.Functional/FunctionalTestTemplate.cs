using System;
using System.Globalization;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using WeeControl.Backend.Domain.BoundedContexts.HumanResources;
using WeeControl.Backend.WebApi.Test.Functional.TestHelpers;
using WeeControl.Common.SharedKernel.Interfaces;
using Xunit;

namespace WeeControl.Backend.WebApi.Test.Functional
{
    public class FunctionalTestTemplate : IClassFixture<CustomWebApplicationFactory<Startup>>, IDisposable
    {
        private static string RandomText => new Random().NextDouble().ToString(CultureInfo.InvariantCulture);

        private readonly HttpClient httpClient;
        private readonly IHumanResourcesDbContext dbContext;
        private readonly string device;

        private Mock<IClientDevice> clientDeviceMock;
        
        public FunctionalTestTemplate(CustomWebApplicationFactory<Startup> factory)
        {
            httpClient = factory.CreateClient();
            
            var scope = factory.Services.GetService<IServiceScopeFactory>().CreateScope();
            dbContext = scope.ServiceProvider.GetService<IHumanResourcesDbContext>();
            
            device = nameof(FunctionalTestTemplate);

            clientDeviceMock = new Mock<IClientDevice>();
            clientDeviceMock.SetupAllProperties();
            clientDeviceMock.Setup(x => x.DeviceId).Returns(device);
        }
        
        public void Dispose()
        {
            clientDeviceMock = null;
        }

        [Fact]
        public async void Example()
        {
            // var token = await RefreshCurrentTokenTests.GetRefreshedTokenAsync(factory, device);
            // clientDeviceMock.Setup(x => x.GetTokenAsync()).ReturnsAsync(token);
            // var service = new AuthenticationService(factory.CreateClient(), clientDeviceMock.Object);
            
            
        }
    }
}
