using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using MySystem.SharedKernel.Enumerators;
using MySystem.SharedKernel.Interfaces;
using MySystem.SharedKernel.Services;
using MySystem.Web.Api.Security.TokenRefreshment;

namespace MySystem.Web.Api.Security.Policies.Territory
{
    public static class CanGetTerritoryPolicy
    {
        private static ISharedValues values = new SharedValues();

        public const string Name = "CanGetTerritoryPolicy"; 

        public static AuthorizationPolicy Policy
        {
            get
            {
                var p = new AuthorizationPolicyBuilder();
                p.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                p.RequireClaim(values.ClaimType[ClaimTypeEnum.Session]);
                p.Requirements.Add(new TokenRefreshmentRequirement(TimeSpan.FromMinutes(100)));

                return p.Build();
            }
        }
    }
}
