using System;
using Moq;
using Sayed.MySystem.ClientService.Services;
using Sayed.MySystem.Shared.Configuration.Models;
using Xunit;

namespace Sayed.MySystem.ClientService.UnitTest.Services
{
    public class ClientServicesSettingTesters
    {
        [Fact]
        public void WhenConstructingTheClass_SettingShouldSplashDisclaimer()
        {
            var service = new ClientServices(null, new Mock<IApi>().Object);

            Assert.NotEmpty(service.Settings.Splash.Disclaimer);
        }

        [Fact]
        public void WhenConstructingTheClass_SettingObjectIsnotNull()
        {
            var service = new ClientServices(null, new Mock<IApi>().Object);

            Assert.NotNull(service.Settings);
        }

        [Fact]
        public void WhenPassingIDeviceToConstructor_HttpClientIsAsRequired()
        {
            string scheme = "Bearer";
            string token = "TokenAsStoredInSecurePlaceInStorage";
            var device = new Mock<IDevice>();
            device.Setup(x => x.Token).Returns(token);

            var service = new ClientServices(device.Object, new Mock<IApi>().Object);

            Assert.NotNull(service.HttpClient);
            Assert.Equal(scheme, service.HttpClient.DefaultRequestHeaders.Authorization.Scheme);
            Assert.Equal(token, service.HttpClient.DefaultRequestHeaders.Authorization.Parameter);
        }
    }
}
