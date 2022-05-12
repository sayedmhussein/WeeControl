using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using WeeControl.Common.UserSecurityLib.Helpers.CustomHandlers.TokenRefreshment;
using Xunit;

namespace WeeControl.Test.UserSecurityLib.Test.CustomHandlers
{
    public class MaximumPeriodHandlerTesters : TokenRefreshmentHandler
    {
        private AuthorizationHandlerContext GetContext(IEnumerable<Claim> claims)
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "Basic"));
            var context = new AuthorizationHandlerContext(new List<IAuthorizationRequirement>(), user, new Document());
            return context;
        }

        [Fact]
        public async void WhenIssuedAtIsNotExceedingSpecificTime_ContextShouldBeSucceded()
        {
            var timeSpan = DateTime.UtcNow - DateTime.UnixEpoch;
            var claims = new List<Claim>() { new Claim("iat", timeSpan.TotalSeconds.ToString()) };
            var context = GetContext(claims);

            await Task.Delay(1000);
            await HandleRequirementAsync(context, new TokenRefreshmentRequirement(TimeSpan.FromSeconds(2)));


            Assert.True(context.HasSucceeded);
        }

        [Fact]
        public async void WhenIssuedAtIsExceedingSpecificTime_ContextShouldNotBeSucceded()
        {
            var timeSpan = DateTime.UtcNow - DateTime.UnixEpoch;
            var claims = new List<Claim>() { new Claim("iat", timeSpan.TotalSeconds.ToString()) };
            var context = GetContext(claims);

            await Task.Delay(3000);
            await HandleRequirementAsync(context, new TokenRefreshmentRequirement(TimeSpan.FromSeconds(2)));


            Assert.False(context.HasSucceeded);
        }
    }
}
