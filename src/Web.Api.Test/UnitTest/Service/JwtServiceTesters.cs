using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MySystem.Web.Api.Service;
using Xunit;

namespace MySystem.Api.Test.UnitTest.Service
{
    public class JwtServiceTesters
    {
        [Fact]
        public void WhenSecurityCodeIsNull_ThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new JwtService(null));
        }

        [Fact]
        public void GenerateTokenWhenNullClaims_ReturnTokenAsString()
        {
            var service = new JwtService(new String('a', 30));

            var token = service.GenerateJwtToken(null, "issuer", DateTime.UtcNow.AddDays(1));

            Assert.NotEmpty(token);
        }

        [Fact]
        public void GenerateTokenWhenNullClaimsAndIssuer_ReturnTokenAsString()
        {
            var service = new JwtService(new String('a', 30));

            var token = service.GenerateJwtToken(null, null, DateTime.UtcNow.AddDays(1));

            Assert.NotEmpty(token);
        }

        [Fact]
        public void GenerateTokenWhenWithSomeClaims_ReturnTokenAsString()
        {
            var service = new JwtService(new String('a', 30));
            var claims = new List<Claim>()
            {
                new Claim("ClaimType", "ClaimValue")
            };

            var token = service.GenerateJwtToken(claims, "issuer", DateTime.UtcNow.AddDays(1));

            Assert.NotEmpty(token);
        }

        [Fact]
        public void WhenInjectingClaimInsideToken_ShoudExtractSameClaimFromToken()
        {
            var secret = new String('a', 30);
            var claim = new Claim("ClaimType", "ClaimValue");

            var service = new JwtService(secret);
            var token = service.GenerateJwtToken(new List<Claim>() { claim }, "issuer", DateTime.UtcNow.AddDays(1));
            var claims = service.GetClaims(token);

            Assert.Contains(claim.Type, claims.Claims.Select(x => x.Type));
            Assert.Contains(claim.Value, claims.Claims.Select(x => x.Value));
        }

        [Fact]
        public void WhenInjectingClaimInsideTokenButDifferentSecret_ShoudThrowAnException()
        {
            var secret = new String('a', 30);
            var claim = new Claim("ClaimType", "ClaimValue");

            var token = new JwtService(secret).GenerateJwtToken(new List<Claim>() { claim }, "issuer", DateTime.UtcNow.AddDays(1));

            Assert.ThrowsAny<Exception>(() => new JwtService(secret + "bla").GetClaims(token));
        }

        [Fact]
        public async void WhenInjectingClaimInsideTokenExpired_ShoudThrowAnException()
        {
            var secret = new String('a', 30);
            var claim = new Claim("ClaimType", "ClaimValue");

            var token = new JwtService(secret).GenerateJwtToken(new List<Claim>() { claim }, "issuer", DateTime.UtcNow.AddSeconds(1));
            await Task.Delay(2000);

            Assert.ThrowsAny<Exception>(() => new JwtService(secret + "bla").GetClaims(token));
        }
    }
}
