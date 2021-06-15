using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using MySystem.SharedKernel.Enumerators.Employee;
using MySystem.SharedKernel.Interfaces.Values;
using MySystem.SharedKernel.Services;

namespace MySystem.Web.Api.Security.Policies.Employee
{
    public static class AbleToRefreshTokenPolicy
    {
        public const string Name = "AbleToRefreshTokenPolicy";
        private static IEmployeeValues values = new EmployeeValues();

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
