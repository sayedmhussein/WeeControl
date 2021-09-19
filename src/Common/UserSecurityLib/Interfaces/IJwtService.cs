using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace WeeControl.UserSecurityLib.Interfaces
{
    /// <summary>
    /// Used by the server to create JWT or extract Claims from token.
    /// </summary>
    public interface IJwtService
    {
        TokenValidationParameters ValidationParameters { get; }

        string GenerateJwtToken(IEnumerable<Claim> claims, string issuer, DateTime expire);

        ClaimsPrincipal GetClaims(string token);
    }
}