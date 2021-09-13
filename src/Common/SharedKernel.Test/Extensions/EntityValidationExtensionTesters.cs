using WeeControl.SharedKernel.DtosV1.Territory;
using WeeControl.SharedKernel.Extensions;
using Xunit;

namespace WeeControl.SharedKernel.Test.Extensions
{
    public class EntityValidationExtensionTesters
    {
        [Fact]
        public void WhenValidTerritoryDto_IsValidIsTrue()
        {
            var dto = new TerritoryDto() { CountryId = "EGP", Name = "EGP" };

            var isValid = dto.IsValid();
            var errors = dto.GetErrorMessages();

            Assert.True(isValid);
            Assert.Empty(errors);
        }

        [Fact]
        public void WhenInValidTerritoryDto_IsValidIsFalseErrorMessageIsNotEmpty()
        {
            var dto = new TerritoryDto() { CountryId = null };

            var isValid = dto.IsValid();
            var errors = dto.GetErrorMessages();

            Assert.False(isValid);
            Assert.NotEmpty(errors);
        }
    }
}
