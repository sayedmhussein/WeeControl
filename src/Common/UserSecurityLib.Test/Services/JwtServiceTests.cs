using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using WeeControl.Common.UserSecurityLib.Interfaces;
using WeeControl.Common.UserSecurityLib.Services;
using Xunit;

namespace WeeControl.Common.UserSecurityLib.Test.Services
{
    public class JwtServiceTests
    {
        private readonly IJwtService jwtService;
        
        public JwtServiceTests()
        {
            var securityKey = new string('a', 30);
            jwtService = new JwtService(securityKey);
        }
        
        [Fact]
        public void WhenSecurityCodeIsNull_ThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new JwtService((string)null));
        }

        [Fact]
        public void GenerateTokenWhenNullClaims_ReturnTokenAsString()
        {
            var token = jwtService.GenerateJwtToken(null, "issuer", DateTime.UtcNow.AddDays(1));

            Assert.NotEmpty(token);
        }

        [Fact]
        public void GenerateTokenWhenNullClaimsAndIssuer_ReturnTokenAsString()
        {
            var token = jwtService.GenerateJwtToken(null, null, DateTime.UtcNow.AddDays(1));

            Assert.NotEmpty(token);
        }

        [Fact]
        public void GenerateTokenWhenWithSomeClaims_ReturnTokenAsString()
        {
            var claims = new List<Claim>()
            {
                new Claim("ClaimType", "ClaimValue")
            };

            var token = jwtService.GenerateJwtToken(claims, "issuer", DateTime.UtcNow.AddDays(1));

            Assert.NotEmpty(token);
        }

        [Fact]
        public void WhenInjectingClaimInsideToken_ShoudExtractSameClaimFromToken()
        {
            var secret = new String('a', 30);
            var claim = new Claim("ClaimType", "ClaimValue");
            
            var token = jwtService.GenerateJwtToken(new List<Claim>() { claim }, "issuer", DateTime.UtcNow.AddDays(1));
            var claims = jwtService.GetClaims(token);

            Assert.Contains(claim.Type, claims.Claims.Select(x => x.Type));
            Assert.Contains(claim.Value, claims.Claims.Select(x => x.Value));
        }

        [Fact]
        public void WhenInjectingClaimInsideTokenButDifferentSecret_ShouldThrowAnException()
        {
            var secret = new String('a', 30);
            var claim = new Claim("ClaimType", "ClaimValue");
            var token = new JwtService(secret).GenerateJwtToken(new List<Claim>() { claim }, "issuer", DateTime.UtcNow.AddDays(1));

            var service = new JwtService(secret + "bla");
            Assert.ThrowsAny<Exception>(() => service.GetClaims(token));
        }

        [Fact]
        public async void WhenInjectingClaimInsideTokenExpired_ShoudThrowAnException()
        {
            var secret = new String('a', 30);
            var claim = new Claim("ClaimType", "ClaimValue");

            var token = jwtService.GenerateJwtToken(new List<Claim>() { claim }, "issuer", DateTime.UtcNow.AddSeconds(1));
            await Task.Delay(2000);

            Assert.ThrowsAny<SecurityTokenInvalidSignatureException>(() => new JwtService(secret + "bla").GetClaims(token));
        }
    }
}
