using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using MySystem.Api.Helpers;

namespace MySystem.Api.Policies
{
    public static class HasRefreshedSession
    {
        public const string Name = "HasRefreshedSession"; 

        public static AuthorizationPolicy Policy { get
            {
                var p = new AuthorizationPolicyBuilder();
                p.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                //p.Requirements.Add(new MaximumPeriodRequirement(TimeSpan.FromMinutes(15)));
                p.RequireClaim("sss");

                return p.Build();
            } }
        
    }
}
