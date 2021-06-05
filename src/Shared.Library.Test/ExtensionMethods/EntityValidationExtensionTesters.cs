using System;
using MySystem.SharedKernel.Dto.V1;
using MySystem.SharedKernel.Entities.Public.Constants;
using MySystem.SharedKernel.Entities.Public.V1Dto;
using MySystem.SharedKernel.ExtensionMethods;
using Xunit;

namespace MySystem.SharedKernel.Test.ExtensionMethods
{
    public class EntityValidationExtensionTesters
    {
        [Fact]
        public void WhenValidDto_ReturnIsValid()
        {
            var dto = new BuildingDto() { BuildingName = "Home", CountryId = Counties.List[Counties.Name.EGYPT] };

            var isValid = dto.IsValid();
            var errors = dto.GetErrorMessages();

            Assert.True(isValid);
            Assert.Empty(errors);
        }

        [Fact]
        public void WhenInValidDto_ReturnIsValidAsFalseAndErrorMessage()
        {
            var dto = new BuildingDto() { BuildingName = "Home" };

            var isValid = dto.IsValid();
            var errors = dto.GetErrorMessages();

            Assert.False(isValid);
            Assert.NotEmpty(errors);
        }
    }
}
