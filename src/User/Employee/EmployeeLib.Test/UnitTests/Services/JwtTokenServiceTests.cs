using System;
using System.Linq;
using System.Collections.Generic;
using System.Security.Claims;
using MySystem.User.Employee.Services;
using Xunit;
namespace MySystem.User.Employee.Test.UnitTests.Services
{
    public class JwtTokenServiceTests
    {
        public JwtTokenServiceTests()
        {
        }

        [Fact]
        public void WhenPassingValidTokenWithClaims_ReturnTheClaims()
        {
            string token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c";
            IEnumerable<Claim> claims = new List<Claim>()
            {
                new Claim("sub", "1234567890"),
                new Claim("name", "John Doe"),
                new Claim("iat", "1516239022")
            };

            var parsedClaims = JwtTokenService.GetClaims(token);

            foreach (var claim in claims)
            {
                Assert.Contains(claim.Type, parsedClaims.Select(x => x.Type).ToList());
                Assert.Contains(claim.Value, parsedClaims.Select(x => x.Value).ToList());
            }
        }

        [Fact]
        public void WhenPassingInvalidToken_ReturnNullList()
        {
            string token = "somebla.bla.bla";

            var claims = JwtTokenService.GetClaims(token);

            Assert.Null(claims);
        }
    }
}