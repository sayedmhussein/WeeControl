using System;
using System.Globalization;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using WeeControl.Backend.Domain.BoundedContexts.Credentials;
using WeeControl.Backend.WebApi.Test.Functional.TestHelpers;
using Xunit;

namespace WeeControl.Backend.WebApi.Test.Functional
{
    public class FunctionalTestTemplate : IClassFixture<CustomWebApplicationFactory<Startup>>, IDisposable
    {
        private static string RandomText => new Random().NextDouble().ToString(CultureInfo.InvariantCulture);

        private readonly HttpClient httpClient;
        private readonly ICredentialsDbContext dbContext;
        private readonly string device;

        //private Mock<IUserDevice> clientDeviceMock;
        
        public FunctionalTestTemplate(CustomWebApplicationFactory<Startup> factory)
        {
            httpClient = factory.CreateClient();
            
            var scope = factory.Services.GetService<IServiceScopeFactory>().CreateScope();
            dbContext = scope.ServiceProvider.GetService<ICredentialsDbContext>();
            
            device = nameof(FunctionalTestTemplate);

            // clientDeviceMock = new Mock<IUserDevice>();
            // clientDeviceMock.SetupAllProperties();
            // clientDeviceMock.Setup(x => x.DeviceId).Returns(device);
        }
        
        public void Dispose()
        {
            //clientDeviceMock = null;
        }

        [Fact]
        public void Example()
        {
            // var token = await RefreshCurrentTokenTests.GetRefreshedTokenAsync(factory, device);
            // clientDeviceMock.Setup(x => x.GetTokenAsync()).ReturnsAsync(token);
            // var service = new AuthenticationService(factory.CreateClient(), clientDeviceMock.Object);
        }
    }
}
