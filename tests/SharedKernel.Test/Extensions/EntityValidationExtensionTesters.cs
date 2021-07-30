using WeeControl.SharedKernel.Common.DtosV1;
using WeeControl.SharedKernel.Extensions;
using Xunit;

namespace WeeControl.SharedKernel.Test.Extensions
{
    public class EntityValidationExtensionTesters
    {
        [Fact]
        public void WhenValidTerritoryDto_IsValidIsTrue()
        {
            var dto = new RequestMetadataV1() { Device = "somestring" };

            var isValid = dto.IsValid();
            var errors = dto.GetErrorMessages();

            Assert.True(isValid);
            Assert.Empty(errors);
        }

        [Fact]
        public void WhenInValidTerritoryDto_IsValidIsFalseErrorMessageIsNotEmpty()
        {
            var dto = new RequestMetadataV1() { Device = null };

            var isValid = dto.IsValid();
            var errors = dto.GetErrorMessages();

            Assert.False(isValid);
            Assert.NotEmpty(errors);
        }
    }
}
