using System;
using MySystem.Shared.Library.Definition;
using MySystem.Shared.Library.Dto.EntityV1;
using MySystem.Shared.Library.ExtensionMethod;
using Xunit;

namespace MySystem.Shared.Library.Test.ExtensionMethods
{
    public class EntityValidationExtensionTesters
    {
        [Fact]
        public void WhenValidDto_ReturnIsValid()
        {
            var dto = new BuildingDto() { BuildingName = "Home", CountryId = Country.Egypt };

            var isValid = dto.IsValid();

            Assert.True(isValid);
        }

        [Fact]
        public void WhenInValidDto_ReturnIsValidAsFalseAndErrorMessage()
        {
            var dto = new BuildingDto() { BuildingName = "Home" };

            var isValid = dto.IsValid();
            var error = dto.ErrorMessage();

            Assert.False(isValid);
            Assert.NotNull(error);
        }
    }
}
