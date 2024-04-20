using WeeControl.Core.DomainModel.Essentials;
using WeeControl.Core.SharedKernel.Exceptions;
using WeeControl.Core.SharedKernel.ExtensionHelpers;

namespace WeeControl.Core.Test.SharedKernel;

public class DomainValidationExceptionTests
{
    [Fact]
    public void TestWhenPersonIsValidatedAndOK_NoException()
    {
        var model = new PersonModel
        {
            FirstName = "FirstName", LastName = "LastName", NationalityCode = "EGP",
            DateOfBirth = new DateTime(2000, 12, 31)
        };

        model.ThrowExceptionIfEntityModelNotValid();
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

        var ex = Assert.Throws<EntityModelValidationException>(() => model.ThrowExceptionIfEntityModelNotValid());

        Assert.Equal(errorCount, ex.Failures.Count);
    }
}