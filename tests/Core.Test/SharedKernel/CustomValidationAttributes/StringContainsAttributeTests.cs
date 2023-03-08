using WeeControl.Core.SharedKernel;
using WeeControl.Core.SharedKernel.CustomValidationAttributes;
using WeeControl.Core.SharedKernel.Exceptions;
using WeeControl.Core.SharedKernel.ExtensionMethods;
using WeeControl.Core.SharedKernel.Interfaces;

namespace WeeControl.Core.Test.SharedKernel.CustomValidationAttributes;

public class StringContainsAttributeTests
{
    [Theory]
    [InlineData("", true)]
    [InlineData("NoSpace", true)]
    [InlineData("AllowedOK", true)]
    [InlineData("Allowed1234", true)]
    [InlineData("Allowed 1234", false)]
    [InlineData("Allowed1234!", false)]
    [InlineData("Allowed1234@", false)]
    public void WhenContainsDefaultValues(string str, bool success)
    {
        var test = new TestObject() {TestString = str};

        if (success)
        {
            Assert.True(test.IsValidEntityModel());
        }
        else
        {
            Assert.Throws<EntityModelValidationException>(() => test.ThrowExceptionIfEntityModelNotValid());
        }
    }
    
    [Theory]
    [InlineData("", true)]
    [InlineData("HelloWorld", true)]
    [InlineData("OKBoss", true)]
    [InlineData("Allowed1234", false)]
    [InlineData("HiYOu 1234", true)]
    [InlineData("Allowed1234!", false)]
    [InlineData("HiYOu@", true)]
    public void WhenContainsSpecialValues(string str, bool success)
    {
        var test = new TestObject2() {TestString = str};

        if (success)
        {
            Assert.True(test.IsValidEntityModel());
        }
        else
        {
            Assert.Throws<EntityModelValidationException>(() => test.ThrowExceptionIfEntityModelNotValid());
        }
    }
}

internal class TestObject : IEntityModel
{
    [StandardString]
    public string TestString { get; set; } = string.Empty;
}

internal class TestObject2 : IEntityModel
{
    [StandardString(Accept = "@ ", Reject = "A")]
    public string TestString { get; set; } = string.Empty;
}