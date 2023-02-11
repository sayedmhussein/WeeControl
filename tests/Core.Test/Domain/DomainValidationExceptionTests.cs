using WeeControl.Core.Domain.Exceptions;
using WeeControl.Core.SharedKernel.Contexts.User;

namespace WeeControl.Core.Test.Domain;

public class DomainValidationExceptionTests
{
    [Fact]
    public void TestWhenPersonIsValidatedAndOK_NoException()
    {
        var model = new PersonModel()
        {
            FirstName = "FirstName", LastName = "LastName", NationalityCode = "EGP", DateOfBirth = new DateOnly(2000, 12, 31)
        };

        DomainValidationException.ValidateEntity(model);
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

        var ex = Assert.Throws<DomainValidationException>(() => DomainValidationException.ValidateEntity(model));
        
        Assert.Equal(errorCount, ex.Failures.Count);
    }
}