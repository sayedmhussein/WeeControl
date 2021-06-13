﻿using MySystem.SharedKernel.EntityV1Dtos.Territory;
using MySystem.SharedKernel.Enumerators;
using MySystem.SharedKernel.ExtensionMethods;
using MySystem.SharedKernel.Interfaces;
using MySystem.SharedKernel.Services;
using Xunit;

namespace MySystem.SharedKernel.Test.ExtensionMethods
{
    public class EntityValidationExtensionTesters
    {
        private readonly ISharedValues values;

        public EntityValidationExtensionTesters()
        {
            values = new SharedValues();
        }

        [Fact]
        public void WhenValidTerritoryDto_IsValidIsTrue()
        {
            var dto = new TerritoryDto() { Name = "Home", CountryId = values.Country[CountryEnum.Egypt]};

            var isValid = dto.IsValid();
            var errors = dto.GetErrorMessages();

            Assert.True(isValid);
            Assert.Empty(errors);
        }

        [Fact]
        public void WhenInValidTerritoryDto_IsValidIsFalseErrorMessageIsNotEmpty()
        {
            var dto = new TerritoryDto() { Name = "Home" };

            var isValid = dto.IsValid();
            var errors = dto.GetErrorMessages();

            Assert.False(isValid);
            Assert.NotEmpty(errors);
        }
    }
}