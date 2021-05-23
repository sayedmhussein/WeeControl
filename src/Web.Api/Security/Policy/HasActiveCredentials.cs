using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using MySystem.Web.Api.Service;

namespace MySystem.Web.Api.Security.Policy
{
    public static class HasActiveCredentials
    {
        public const string Name = "HasActiveCredentials"; 

        public static AuthorizationPolicy Policy
        {
            get
            {
                var p = new AuthorizationPolicyBuilder();
                p.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                p.RequireClaim("sss");

                return p.Build();
            }
        }
    }
}
