using System.ComponentModel.DataAnnotations;
using WeeControl.Core.Domain.Contexts.User;
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
            FirstName = "FirstName", LastName = "LastName", Nationality = "EGP", DateOfBirth = new DateOnly(2000, 12, 31)
        };
        
        var person = PersonDbo.Create(model);
        
        DomainValidationException.ValidateEntity(person);
    }

    [Theory]
    [InlineData("", "LastName", "EGP", 1)]
    public void TestWhenFailures_ExceptionThrown(string firstName, string lastName, string nationality, int errorCount)
    {
        var model = new PersonModel()
        {
            FirstName = firstName, LastName = lastName, Nationality = nationality, DateOfBirth = new DateOnly(2000, 12, 31)
        };
         
        var person = PersonDbo.Create(model);
        
        Assert.ThrowsAny<DomainValidationException>(() =>
        {
            DomainValidationException.ValidateEntity(person);
        });
    }
}