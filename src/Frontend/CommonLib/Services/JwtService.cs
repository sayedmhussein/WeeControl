﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using WeeControl.SharedKernel.Interfaces;

namespace WeeControl.Frontend.CommonLib.Services
{
    public class JwtService : IJwtService
    {
        private readonly string securityKey;

        public JwtService(string securityKey)
        {
            this.securityKey = securityKey ?? throw new ArgumentNullException("Security Key must have value.");
        }

        public TokenValidationParameters ValidationParameters
        {
            get
            {
                var paremeters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = false,
                    //TryAllIssuerSigningKeys = true,
                    //IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(securityKey)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    //ValidAudience = Configuration["Jwt:Audience"],
                    ValidateLifetime = false,
                    ClockSkew = TimeSpan.Zero
                };

                return paremeters;
            }
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
            var bla = handler.ReadJwtToken(token);
            var cla = bla.Claims;
            var ida = new ClaimsIdentity("Custom");
            ida.AddClaims(cla);
            
            return new ClaimsPrincipal(ida);
        }
    }
}