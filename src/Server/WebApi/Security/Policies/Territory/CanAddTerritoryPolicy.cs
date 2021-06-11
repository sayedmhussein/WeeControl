using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using MySystem.SharedKernel.Interfaces;
using MySystem.SharedKernel.Services;
using MySystem.SharedKernel.Enumerators;
using MySystem.Web.Api.Security.TokenRefreshment;

namespace MySystem.Web.Api.Security.Policies.Territory
{
    public static class CanAddTerritoryPolicy
    {
        private static ISharedValues values = new SharedValues();

        public const string Name = "CanAddTerritoryPolicy"; 

        public static AuthorizationPolicy Policy
        {
            get
            {

                var p = new AuthorizationPolicyBuilder();
                p.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                p.RequireClaim(values.ClaimType[ClaimTypeEnum.Session]);
                p.RequireClaim(values.ClaimType[ClaimTypeEnum.HumanResources], values.ClaimTag[ClaimTagEnum.Add]);
                p.Requirements.Add(new TokenRefreshmentRequirement(TimeSpan.FromMinutes(5)));

                return p.Build();
            }
        }
    }
}
