using System;
using System.ComponentModel.DataAnnotations;
using MySystem.Web.Api.Service;
using Xunit;

namespace MySystem.Api.Test.UnitTest.Service
{
    public class ValidationServiceTesters
    {
        [Fact]
        public void WhenEverythingOK_ReturnValidatedWithNoError()
        {
            var obj = new TestClass() { Id = 4, Name = "Name Of Me" };

            var validator = new ValidationService<TestClass>(obj);

            Assert.True(validator.IsValid);
            Assert.Null(validator.ErrorMessage);
        }

        [Fact]
        public void WhenNullingName_ReturnNotValidatedWithError()
        {
            var obj = new TestClass() { Name = null };

            var validator = new ValidationService<TestClass>(obj);

            Assert.False(validator.IsValid);
            Assert.NotNull(validator.ErrorMessage);
        }

        [Fact]
        public void WhenNameExceedSize_ReturnNotValidatedWithError()
        {
            var obj = new TestClass() { Id = 4, Name = "1234567891011" };

            var validator = new ValidationService<TestClass>(obj);

            Assert.False(validator.IsValid);
            Assert.NotNull(validator.ErrorMessage);
        }
    }

    public class TestClass
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(10, ErrorMessage ="Error")]
        public string Name { get; set; }
    }
}
