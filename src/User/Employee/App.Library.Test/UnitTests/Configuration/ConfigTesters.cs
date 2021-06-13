using MySystem.User.Employee.Configuration;
using Xunit;

namespace MySystem.User.Employee.Test.UnitTests.Configuration
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
