using System;
using Xunit;
using Moq;
using Sayed.MySystem.ClientService.Services;
using Sayed.MySystem.Shared.Configuration.Models;
using Sayed.MySystem.ClientService.Configuration;

namespace Sayed.MySystem.ClientService.UnitTest.Configuration
{
    public class ConfigTesters
    {
        [Fact]
        public void WhenClientServiceIsConstructed_ClassSettingShouldNotBeNull()
        {
            var instance = Config.GetInstance();

            Assert.NotNull(instance);
        }

        [Fact]
        public void SplashDisclaimerShouldNotBeNull()
        {
            var instance = Config.GetInstance();

            Assert.NotNull(instance.Splash.Disclaimer);
        }

        [Fact]
        public void LoginDisclaimerShouldNotBeEmpty()
        {
            var instance = Config.GetInstance();

            Assert.NotEmpty(instance.Login.Disclaimer);
        }

        [Fact]
        public void HomeDisclaimerShouldNotBeEmpty()
        {
            var instance = Config.GetInstance();

            Assert.NotEmpty(instance.Home.Text);
        }
    }
}
