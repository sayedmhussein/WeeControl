using System;
using MySystem.SharedKernel.Definition;
using MySystem.SharedKernel.Dto.V1;
using MySystem.SharedKernel.ExtensionMethod;
using Xunit;

namespace MySystem.SharedKernel.Test.ExtensionMethods
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
