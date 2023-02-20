using WeeControl.Core.SharedKernel;
using WeeControl.Core.SharedKernel.Contexts.User;

namespace WeeControl.Core.Test.SharedKernel;

public class ModelValidationErrorTests
{
    [Fact]
    public void TestWhenPersonIsValidatedAndOK_NoException()
    {
        var model = new PersonModel()
        {
            FirstName = "FirstName", LastName = "LastName", NationalityCode = "EGP", DateOfBirth = new DateOnly(2000, 12, 31)
        };

        Assert.Empty(model.GetModelValidationError());
    }

    [Theory]
    [InlineData("", "LastName", "EGP", 1)]
    [InlineData("", "", "EGP", 2)]
    [InlineData("FirstName", "LastName", "EGYPT", 1)]
    [InlineData("FirstName", "LastName", "EG", 1)]
    public void TestWhenFailures_ExceptionThrown(string firstName, string lastName, string nationality, int errorCount)
    {
        var model = new PersonModel()
        {
            FirstName = firstName, LastName = lastName, NationalityCode = nationality, DateOfBirth = new DateOnly(2000, 12, 31)
        };

        var ex = model.GetModelValidationError();
        
        Assert.Equal(errorCount, ex.Count);
    }

}