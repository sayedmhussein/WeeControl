using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using WeeControl.SharedKernel.Interfaces;
using WeeControl.SharedKernel.Services;
using Xunit;

namespace WeeControl.SharedKernel.Test.Services;

public class JwtServiceTests : IDisposable
{
    private IJwtService service;
    private readonly string securityKey = new string('a', 30);
        
    public JwtServiceTests()
    {
        service = new JwtService();
    }

    public void Dispose()
    {
        service = null;
    }

    [Theory]
    [InlineData("Type", "Value")]
    [InlineData("Type", "")]
    public void WhenGeneratingATokenUsingClaims_WhenExtractingSameClaimShouldBeExist(string type, string value)
    {
        var key = Encoding.ASCII.GetBytes(securityKey);
        var claim = new Claim(type, value);
        var list = new List<Claim>() { claim };

        var descriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(list),
            IssuedAt = DateTime.UtcNow,
            Expires = DateTime.UtcNow.AddMinutes(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        };

        var token = service.GenerateToken(descriptor);
            
        Assert.NotEmpty(token);

        var parameters = new TokenValidationParameters()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateLifetime = true,
            ValidateAudience = false,
            ValidateIssuer = false
        };

        var cp = service.GetClaimPrincipal(token, parameters);
            
        Assert.Equal(type, cp.Claims.First().Type);
        Assert.Equal(value, cp.Claims.First().Value);
    }
}