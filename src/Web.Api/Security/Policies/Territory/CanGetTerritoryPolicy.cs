using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using MySystem.Web.Api.Security.TokenRefreshment;

namespace MySystem.Web.Api.Security.Policies.Territory
{
    public static class CanGetTerritoryPolicy
    {
        public const string Name = "CanGetTerritoryPolicy"; 

        public static AuthorizationPolicy Policy
        {
            get
            {
                var p = new AuthorizationPolicyBuilder();
                p.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                p.RequireClaim(Claims.Types[Claims.ClaimType.Session], Claims.Tags[Claims.ClaimTag.Add]);
                p.Requirements.Add(new TokenRefreshmentRequirement(TimeSpan.FromMinutes(100)));

                return p.Build();
            }
        }
    }
}
