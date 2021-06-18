using WeeControl.SharedKernel.BasicSchemas.Common.DtosV1;
using WeeControl.SharedKernel.BasicSchemas.Common.Extensions;
using Xunit;

namespace WeeControl.SharedKernel.Test.Schemas.Common.Extensions
{
    public class EntityValidationExtensionTesters
    {
        [Fact]
        public void WhenValidTerritoryDto_IsValidIsTrue()
        {
            var dto = new RequestMetadata() { Device = "somestring" };

            var isValid = dto.IsValid();
            var errors = dto.GetErrorMessages();

            Assert.True(isValid);
            Assert.Empty(errors);
        }

        [Fact]
        public void WhenInValidTerritoryDto_IsValidIsFalseErrorMessageIsNotEmpty()
        {
            var dto = new RequestMetadata() { Device = null };

            var isValid = dto.IsValid();
            var errors = dto.GetErrorMessages();

            Assert.False(isValid);
            Assert.NotEmpty(errors);
        }
    }
}
