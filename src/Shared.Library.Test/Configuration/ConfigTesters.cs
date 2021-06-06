using System;
using MySystem.SharedKernel.Configuration;
using Xunit;

namespace MySystem.SharedKernel.Test.Configuration
{
    public class ConfigTesters
    {
        [Fact]
        public void WhenGettingAppSettingObject_ObjectMustNoBeNull()
        {
            var obj = Config.AppSettingObject;

            Assert.NotNull(obj);
        }
    }
}
