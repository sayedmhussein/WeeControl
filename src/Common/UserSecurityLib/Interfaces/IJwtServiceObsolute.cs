using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace WeeControl.Common.UserSecurityLib.Interfaces
{
    [Obsolete]
    public interface IJwtServiceObsolute
    {
        string GenerateJwtToken(IEnumerable<Claim> claims, string issuer, DateTime expire);

        ClaimsPrincipal GetClaims(string token, bool isValidated = true);
    }
}