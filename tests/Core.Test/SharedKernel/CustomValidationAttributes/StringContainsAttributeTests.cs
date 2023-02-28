using WeeControl.Core.SharedKernel;
using WeeControl.Core.SharedKernel.CustomValidationAttributes;
using WeeControl.Core.SharedKernel.Interfaces;

namespace WeeControl.Core.Test.SharedKernel.CustomValidationAttributes;

public class StringContainsAttributeTests
{
    [Theory]
    [InlineData("")]
    [InlineData("NoSpace")]
    [InlineData("AllowedOK")]
    [InlineData("Allowed1234")]
    public void WhenContainsValidValue_NoError(string str)
    {
        var test = new TestObject() {TestString = str};

        Assert.True(test.IsValidEntityModel());
    }
    
    [Fact]
    public void WhenContainsInvalidValidValue_ThrowError()
    {
        var test = new TestObject() { TestString = "Allowed ! OK"};

        Assert.Throws<EntityModelValidationException>(() => test.ThrowExceptionIfEntityModelNotValid());
    }
}

internal class TestObject : IEntityModel
{
    [StringContains]
    public string TestString { get; set; } = string.Empty;
}