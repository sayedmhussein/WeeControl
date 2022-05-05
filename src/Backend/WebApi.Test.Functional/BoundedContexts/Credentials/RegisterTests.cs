using System;
using WeeControl.Backend.WebApi.Test.Functional.TestHelpers;
using Xunit;
using Moq;
using WeeControl.Common.SharedKernel.Interfaces;

namespace WeeControl.Backend.WebApi.Test.Functional.BoundedContexts.Credentials
{
    public class RegisterTests : IClassFixture<CustomWebApplicationFactory<Startup>>, IDisposable
    {
        private readonly CustomWebApplicationFactory<Startup> factory;
        private Mock<IUserDevice> device;

        public RegisterTests(CustomWebApplicationFactory<Startup> factory)
        {
            this.factory = factory;

            device = new();
            device.SetupAllProperties();
            //device.Setup(x => x.ServerBaseAddress).Returns("http://10.0.2.2:5000/");
            //device.Setup(x => x.ServerBaseAddress).Returns(CustomWebApplicationFactory<Startup>.GetLocalIPAddress());
            device.Setup(x => x.DeviceId).Returns(nameof(RegisterTests));
            device.Setup(x => x.TimeStamp).Returns(DateTime.UtcNow);
        }

        public void Dispose()
        {
            device = null;
        }

        [Fact]
        public async void RegisterTest()
        {
            var client = factory.CreateClient();
            //var operation = new UserOperation(device.Object, client);
            //var response = await operation.RegisterAsync(new RegisterDto() { Username = new Random().NextDouble().ToString(), Password = new Random().NextDouble().ToString() });
            

            //Assert.Equal(HttpStatusCode.OK, response.StatuesCode);
            //Assert.NotEmpty(response.Payload.Token);
        }
    }
}
