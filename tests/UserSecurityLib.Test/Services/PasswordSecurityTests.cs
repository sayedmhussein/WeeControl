using WeeControl.Common.SharedKernel.Interfaces;
using WeeControl.Common.SharedKernel.Services;
using Xunit;

namespace WeeControl.Test.UserSecurityLib.Test.Services;

public class PasswordSecurityTests
{
    private readonly IPasswordSecurity passwordSecurity;

    public PasswordSecurityTests()
    {
        passwordSecurity = new PasswordSecurity();
    }

    [Fact]
    public void WhenNewPasswordIsCreated_ShouldReturnHashedString()
    {
        const string plainPassword = "Password";

        var hash = passwordSecurity.Hash(plainPassword);
        
        Assert.NotEmpty(hash);
    }
    
    [Fact]
    public void WhenVerifyingCorrectPassword_ShouldReturnTrue()
    {
        const string plainPassword = "Password";
        var hash = passwordSecurity.Hash(plainPassword);

        var isValid = passwordSecurity.Verify(plainPassword, hash);
        
        Assert.True(isValid);
    }

    [Fact]
    public void WhenVerifyingInvalidPassword_ShouldReturnFalse()
    {
        const string plainPassword = "Password";

        var isValid = passwordSecurity.Verify(plainPassword, "Invalid");
        
        Assert.False(isValid);
    }
}