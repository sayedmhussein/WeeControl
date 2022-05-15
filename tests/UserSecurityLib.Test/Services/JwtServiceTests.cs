using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using WeeControl.Common.SharedKernel.Interfaces;
using WeeControl.Common.SharedKernel.Services;
using Xunit;

namespace WeeControl.Test.UserSecurityLib.Test.Services;

public class JwtServiceTests : IDisposable
{
    private IJwtService service;
    private string securityKey = new string('a', 30);
        
    public JwtServiceTests()
    {
        service = new JwtService();
    }

    public void Dispose()
    {
        service = null;
    }

    [Fact]
    public void WhenGeneratingATokenUsingClaims_WhenExtractingSameClaimShouldBeExist()
    {
        var key = Encoding.ASCII.GetBytes(securityKey);
        var claim = new Claim("Type", "Value");
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

        var claimPrincible = service.ExtractClaimPrincipal(parameters, token);
            
        Assert.Equal("Type", claimPrincible.Claims.First().Type);
        Assert.Equal("Value", claimPrincible.Claims.First().Value);
    }
}