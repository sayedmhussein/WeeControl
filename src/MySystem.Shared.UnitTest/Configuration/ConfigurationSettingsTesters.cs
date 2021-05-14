using System;
using Sayed.MySystem.Shared.Configuration;
using Xunit;

namespace Sayed.MySystem.Shared.UnitTest.Configuration
{
    public class ConfigurationSettingsTesters
    {
        [Fact]
        public void WhenConstructingTheClass_SettingShouldReturnValidApiBaseValue()
        {
            var settings = AppSettings.GetAppSetting();

            Assert.Contains("http://", settings.Api.Base.ToString());
        }

        [Fact]
        public void WhenConstructingTheClass_SettingObjectIsnotNull()
        {
            var settings = AppSettings.GetAppSetting();

            Assert.NotNull(settings);
        }
    }
}
