using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace WeeControl.Backend.Application.Common.Interfaces
{
    public interface IJwtService
    {
        TokenValidationParameters ValidationParameters { get; }

        string GenerateJwtToken(IEnumerable<Claim> claims, string issuer, DateTime expire);

        ClaimsPrincipal GetClaims(string token);
    }
}