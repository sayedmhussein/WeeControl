using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using WeeControl.Server.WebApi.Security.TokenRefreshment.CustomHandlers;

namespace WeeControl.Server.WebApi.Security.Policies.Employee
{
    public static class AbleToDeleteExisingEmployeePolicy
    {
        public const string Name = "AbleToDeleteExisingEmployeePolicy"; 

        public static AuthorizationPolicy Policy
        {
            get
            {
                var p = new AuthorizationPolicyBuilder();
                p.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                //p.RequireClaim(Claims.Types[Claims.ClaimType.Session], Claims.Tags[Claims.ClaimTag.Add]);
                p.Requirements.Add(new TokenRefreshmentRequirement(TimeSpan.FromMinutes(5)));
                
                return p.Build();
            }
        }
    }
}
