using WeeControl.Core.SharedKernel.CustomValidationAttributes;
using WeeControl.Core.SharedKernel.Exceptions;
using WeeControl.Core.SharedKernel.ExtensionMethods;
using WeeControl.Core.SharedKernel.Interfaces;

namespace WeeControl.Core.Test.SharedKernel.CustomValidationAttributes;

public class StandardPasswordAttributeTests
{
    [Theory]
    [InlineData("", false)]
    [InlineData(" ", false)]
    [InlineData("-", false)]
    [InlineData("now_capital_letter@123", false)]
    [InlineData("NoSpecialLetter123", false)]
    [InlineData("NoNumber@N", false)]
    [InlineData("NOSMALLLETTER@123", false)]
    [InlineData("sml@5", false)]
    [InlineData("OneCapitalOneSmallSpecialAndNumber#123", true)]
    public async void Tests(string password, bool success)
    {
        var test = new TestObject {TestString = password};

        if (success)
            Assert.True(test.IsValidEntityModel());
        else
            Assert.Throws<EntityModelValidationException>(() => test.ThrowExceptionIfEntityModelNotValid());
    }

    private class TestObject : IEntityModel
    {
        [StandardPassword] 
        public string TestString { get; set; } = string.Empty;
    }
}