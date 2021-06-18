using System;
using WeeControl.SharedKernel.Configurations;
using Xunit;

namespace WeeControl.SharedKernel.Test.Configurations
{
    public class SettingsTests
    {
        [Fact]
        public void AppSettingsFullNamespaceNameIsNotNull()
        {
            var name = Settings.Filename;

            Assert.NotNull(name);
            Assert.NotEmpty(name);
        }
    }
}
