using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using MySystem.SharedKernel.Definition;

namespace MySystem.Web.Api.Security.Policy
{
    public static class SessionNotBlockedPolicy
    {
        public const string Name = "HasActiveCredentials"; 

        public static AuthorizationPolicy Policy
        {
            get
            {
                var p = new AuthorizationPolicyBuilder();
                p.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                p.RequireClaim(UserClaim.Session);

                return p.Build();
            }
        }
    }
}
