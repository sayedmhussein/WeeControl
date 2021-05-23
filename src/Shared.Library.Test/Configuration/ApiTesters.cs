using System;
using MySystem.Shared.Library.Configuration;
using Xunit;

namespace MySystem.Shared.Library.Test.Configuration
{
    public class ConfigurationSettingsTesters
    {
        [Fact]
        public void WhenConstructingTheClass_SettingShouldReturnValidApiBaseValue()
        {
            var settings = Api.GetAppSetting();

            Assert.Contains("http://", settings.Base.ToString());
        }

        [Fact]
        public void WhenConstructingTheClass_SettingObjectIsnotNull()
        {
            var settings = Api.GetAppSetting();

            Assert.NotNull(settings);
        }
    }
}
