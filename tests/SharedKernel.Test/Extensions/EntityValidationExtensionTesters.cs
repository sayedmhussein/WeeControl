using Xunit;

namespace WeeControl.SharedKernel.Test.Extensions
{
    public class EntityValidationExtensionTesters
    {
        [Fact]
        public void WhenValidTerritoryDto_IsValidIsTrue()
        {
            // var dto = new Dto() { CountryCode = "EGP", Name = "EGP" };
            //
            // var isValid = dto.IsValid();
            // var errors = dto.GetErrorMessages();
            //
            // Assert.True(isValid);
            // Assert.Empty(errors);
        }

        [Fact]
        public void WhenInValidTerritoryDto_IsValidIsFalseErrorMessageIsNotEmpty()
        {
            // var dto = new Dto() { CountryCode = null };
            //
            // var isValid = dto.IsValid();
            // var errors = dto.GetErrorMessages();
            //
            // Assert.False(isValid);
            // Assert.NotEmpty(errors);
        }
    }
}
