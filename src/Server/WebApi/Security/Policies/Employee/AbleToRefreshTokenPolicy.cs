using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using WeeControl.SharedKernel.CommonSchemas.Employee.Dicts;
using WeeControl.SharedKernel.CommonSchemas.Employee.Enums;

namespace WeeControl.Server.WebApi.Security.Policies.Employee
{
    public static class AbleToRefreshTokenPolicy
    {
        public const string Name = "AbleToRefreshTokenPolicy";
        private static IClaimDicts values = new ClaimDicts();

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
