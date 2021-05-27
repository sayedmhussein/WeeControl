using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using MySystem.Web.Domain.Security.Requirement;

namespace MySystem.Web.Domain.Security.Policy
{
    public static class TokenRefreshmentPolicy
    {
        public const string Name = "HasValidSessionAndAccount"; 

        public static AuthorizationPolicy Policy
        {
            get
            {
                var p = new AuthorizationPolicyBuilder();
                p.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                p.RequireClaim("sss");
                p.Requirements.Add(new SessionNotBlockedRequirement());
                
                return p.Build();
            }
        }
    }
}
