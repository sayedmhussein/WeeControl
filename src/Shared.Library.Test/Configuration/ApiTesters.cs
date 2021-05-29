using System;
using MySystem.SharedKernel.Configuration;
using Xunit;

namespace MySystem.SharedKernel.Library.Test.Configuration
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
