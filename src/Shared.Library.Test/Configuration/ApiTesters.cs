using System;
using MySystem.Shared.Library.Configuration;
using Xunit;

namespace MySystem.Shared.Library.Test.Configuration
{
    public class ConfigurationSettingsTesters
    {
        [Fact]
        public void WhenConstructingTheClass_ApiShouldReturnValidApiBaseValue()
        {
            var api = Api.GetAppSetting();

            Assert.Contains("http://", api.Base.ToString());
        }

        [Fact]
        public void WhenConstructingTheClass_SettingObjectIsnotNull()
        {
            var api = Api.GetAppSetting();

            Assert.NotNull(api);
        }
    }
}
