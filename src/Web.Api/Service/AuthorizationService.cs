using Microsoft.AspNetCore.Authorization;
using MySystem.MySystem.Api.Security.Policies;
using MySystem.Web.Api.Security.Policies.Employee;
using MySystem.Web.Api.Security.Policies.Territory;

namespace MySystem.MySystem.Api.Service
{
    public class AuthorizationService
    {
        public static void ConfigureAuthorizationOptions(AuthorizationOptions options)
        {
            options.AddPolicy(nameof(TerritoryPolicies.PolicyName.CanGetPolicy), TerritoryPolicies.Policies[TerritoryPolicies.PolicyName.CanGetPolicy]);


            options.AddPolicy(CanGetTerritoryPolicy.Name, CanGetTerritoryPolicy.Policy);
            options.AddPolicy(CanAddEditTerritoryPolicy.Name, CanAddEditTerritoryPolicy.Policy);
            options.AddPolicy(CanDeleteTerritoryPolicy.Name, CanDeleteTerritoryPolicy.Policy);

            options.AddPolicy(AbleToAddNewEmployeePolicy.Name, AbleToAddNewEmployeePolicy.Policy);
        }
    }
}
