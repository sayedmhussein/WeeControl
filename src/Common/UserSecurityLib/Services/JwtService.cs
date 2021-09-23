using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using WeeControl.Common.UserSecurityLib.Interfaces;

namespace WeeControl.Common.UserSecurityLib.Services
{
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

        public ClaimsPrincipal ExtractClaimPrincipal(TokenValidationParameters parameters, string token)
        {
            var handler = new JwtSecurityTokenHandler();

            return handler.ValidateToken(token, parameters, out var _);
        }
    }
}