using WeeControl.SharedKernel.Configurations;
using Xunit;

namespace WeeControl.SharedKernel.Test.Configurations
{
    public class AppSettingsTests : AppSettings
    {
        [Fact]
        public void JsonFileIsNotNull()
        {
            var file = json;

            Assert.NotNull(file);
        }

        [Fact]
        public void JsonFileIsNotEmpty()
        {
            var file = json;

            Assert.NotEmpty(file);
        }
    }
}
