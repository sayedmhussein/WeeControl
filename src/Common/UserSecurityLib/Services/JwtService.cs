using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using WeeControl.Common.UserSecurityLib.Interfaces;

namespace WeeControl.Common.UserSecurityLib.Services
{
    public class JwtService : IJwtService
    {
        private readonly string securityKey;

        public JwtService(string securityKey)
        {
            this.securityKey = securityKey ?? throw new ArgumentNullException("Security Key must have value.");

            ValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                //TryAllIssuerSigningKeys = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(securityKey)),
                ValidateIssuer = false,
                ValidateAudience = false,
                //ValidAudience = Configuration["Jwt:Audience"],
                ValidateLifetime = false,
                ClockSkew = TimeSpan.Zero
            };
        }

        public JwtService(IConfiguration configuration) : this(configuration["Jwt:Key"])
        {
        }
        
        public TokenValidationParameters ValidationParameters { get; set; }

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
            var securityToken = handler.ReadJwtToken(token);
            var securityTokenClaims = securityToken.Claims;
            var claimsIdentity = new ClaimsIdentity("Custom");
            claimsIdentity.AddClaims(securityTokenClaims);
            
            return new ClaimsPrincipal(claimsIdentity);
        }
    }
}
