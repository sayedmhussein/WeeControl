using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using WeeControl.SharedKernel.Interfaces;

namespace WeeControl.SharedKernel.Services;

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

    public ClaimsPrincipal ExtractClaimPrincipalWithoutValidationParameter(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var securityToken = handler.ReadJwtToken(token);
        var ci = new ClaimsIdentity("custom");
        ci.AddClaims(securityToken.Claims);
        var cp = new ClaimsPrincipal(ci);
        return cp;
    }

    public ClaimsPrincipal ExtractClaimPrincipalWithValidationParameter(string token, TokenValidationParameters parameters)
    {
        var handler = new JwtSecurityTokenHandler();
        return handler.ValidateToken(token, parameters, out var _);
    }
}