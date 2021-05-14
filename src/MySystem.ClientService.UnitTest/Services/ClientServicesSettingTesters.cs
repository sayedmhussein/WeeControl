using System;
using Moq;
using Sayed.MySystem.ClientService.Services;
using Xunit;

namespace Sayed.MySystem.ClientService.UnitTest.Services
{
    public class ClientServicesSettingTesters
    {
        [Fact]
        public void WhenConstructingTheClass_SettingShouldReturnValidApiBaseValue()
        {
            var service = new ClientServices(null);

            Assert.Contains("http://", service.Settings.Api.Base.ToString());
        }

        [Fact]
        public void WhenConstructingTheClass_SettingObjectIsnotNull()
        {
            var service = new ClientServices(null);

            Assert.NotNull(service.Settings);
        }

        [Fact]
        public void WhenPassingIDeviceToConstructor_HttpClientIsAsRequired()
        {
            string scheme = "Bearer";
            string token = "TokenAsStoredInSecurePlaceInStorage";
            var device = new Mock<IDevice>();
            device.Setup(x => x.Token).Returns(token);

            var service = new ClientServices(device.Object);

            Assert.NotNull(service.HttpClient);
            Assert.Equal(scheme, service.HttpClient.DefaultRequestHeaders.Authorization.Scheme);
            Assert.Equal(token, service.HttpClient.DefaultRequestHeaders.Authorization.Parameter);
        }
    }
}
