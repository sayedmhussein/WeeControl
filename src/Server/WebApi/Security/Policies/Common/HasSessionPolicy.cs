using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using WeeControl.SharedKernel.BasicSchemas.Employee.Dicts;
using WeeControl.SharedKernel.BasicSchemas.Employee.Enums;

namespace WeeControl.Server.WebApi.Security.Policies.Common
{
    public static class HasSessionPolicy
    {
        private static readonly IClaimDicts values = new ClaimDicts();

        public const string Name = "HasSessionPolicy";

        public static AuthorizationPolicy Policy
        {
            get
            {
                var p = new AuthorizationPolicyBuilder();
                p.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                p.RequireClaim(values.ClaimType[ClaimTypeEnum.Session]);

                return p.Build();
            }
        }
    }
}
