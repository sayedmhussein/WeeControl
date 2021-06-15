using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using MySystem.SharedKernel.Enumerators.Employee;
using MySystem.SharedKernel.Interfaces.Values;
using MySystem.SharedKernel.Services;
using MySystem.Web.Api.Security.TokenRefreshment;
using MySystem.Web.Api.Security.TokenRefreshment.CustomHandlers;

namespace MySystem.Web.Api.Security.Policies.Territory
{
    public static class CanGetTerritoryPolicy
    {
        private static IEmployeeValues values = new EmployeeValues();

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
