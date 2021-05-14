using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Sayed.MySystem.Api.Helpers
{
    public class JwtOperation
    {
        private readonly string securityKey;

        public JwtOperation(string securityKey)
        {
            this.securityKey = securityKey ?? throw new ArgumentNullException("Security Key must have value.");
        }

        public string GenerateJwtToken(IEnumerable<Claim> claims, string issuer, DateTime expire)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(securityKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expire,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256),
                Issuer = issuer,
                Audience = "",
                IssuedAt = DateTime.UtcNow
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public ClaimsPrincipal GetClaims(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            //
            var key = Encoding.ASCII.GetBytes(securityKey);
            var validations = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            var claims = handler.ValidateToken(token, validations, out var tokenSecure);
            return claims;
        }
    }
}
