using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using WeeControl.Core.SharedKernel.Interfaces;

namespace WeeControl.Core.SharedKernel.Services;

public class JwtService : IJwtService
{
    public string GenerateToken(SecurityTokenDescriptor descriptor)
    {
        descriptor.IssuedAt ??= DateTime.UtcNow;
        descriptor.Issuer ??= nameof(WeeControl);

        var tokenHandler = new JwtSecurityTokenHandler();

        var token = tokenHandler.CreateToken(descriptor);
        return tokenHandler.WriteToken(token);
    }

    public ClaimsPrincipal GetClaimPrincipal(string token, TokenValidationParameters? parameters)
    {
        if (string.IsNullOrEmpty(token)) return new ClaimsPrincipal();

        if (parameters != null)
        {
            var handler1 = new JwtSecurityTokenHandler();
            return handler1.ValidateToken(token, parameters, out var _);
        }

        var handler2 = new JwtSecurityTokenHandler();
        var securityToken = handler2.ReadJwtToken(token);
        var ci = new ClaimsIdentity("custom");
        ci.AddClaims(securityToken.Claims);
        var cp = new ClaimsPrincipal(ci);
        return cp;
    }
}