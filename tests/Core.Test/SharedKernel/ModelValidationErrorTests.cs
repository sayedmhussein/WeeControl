using WeeControl.Core.SharedKernel.Contexts.Essentials;
using WeeControl.Core.SharedKernel.ExtensionMethods;

namespace WeeControl.Core.Test.SharedKernel;

public class ModelValidationErrorTests
{
    [Fact]
    public void TestWhenPersonIsValidatedAndOK_NoException()
    {
        var model = new PersonModel
        {
            FirstName = "FirstName", LastName = "LastName", NationalityCode = "EGP",
            DateOfBirth = new DateTime(2000, 12, 31)
        };

        Assert.Empty(model.GetModelValidationErrors());
        Assert.True(model.IsValidEntityModel());
        Assert.Empty(model.GetFirstValidationError());
    }

    [Theory]
    [InlineData("", "LastName", "EGP", 1)]
    [InlineData("", "", "EGP", 2)]
    [InlineData("FirstName", "LastName", "EGYPT", 1)]
    [InlineData("FirstName", "LastName", "EG", 1)]
    public void TestWhenFailures_ExceptionThrown(string firstName, string lastName, string nationality, int errorCount)
    {
        var model = new PersonModel
        {
            FirstName = firstName, LastName = lastName, NationalityCode = nationality,
            DateOfBirth = new DateTime(2000, 12, 31)
        };

        var ex = model.GetModelValidationErrors();

        Assert.Equal(errorCount, ex.Count);
        Assert.False(model.IsValidEntityModel());
        Assert.NotEmpty(model.GetFirstValidationError());
    }
}