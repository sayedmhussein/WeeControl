using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using MySystem.Web.Api.Service;
using MySystem.Web.Api.Security.Requirement;

namespace MySystem.Web.Api.Security.Policy
{
    public static class HasValidSessionAndAccount
    {
        public const string Name = "HasValidSessionAndAccount"; 

        public static AuthorizationPolicy Policy
        {
            get
            {
                var p = new AuthorizationPolicyBuilder();
                p.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                p.RequireClaim("sss");
                p.Requirements.Add(new SessionAndUserValidRequirement());
                
                return p.Build();
            }
        }
    }
}
