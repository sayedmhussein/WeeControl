using Microsoft.AspNetCore.Authorization;
using MySystem.Web.Api.Security.Policies.Employee;
using MySystem.Web.Api.Security.Policies.Territory;

namespace MySystem.MySystem.Api.Service
{
    public class AuthorizationService
    {
        public static void ConfigureAuthorizationOptions(AuthorizationOptions options)
        {
            options.AddPolicy(CanGetTerritoryPolicy.Name, CanGetTerritoryPolicy.Policy);
            options.AddPolicy(CanAddEditTerritoryPolicy.Name, CanAddEditTerritoryPolicy.Policy);
            options.AddPolicy(CanDeleteTerritoryPolicy.Name, CanDeleteTerritoryPolicy.Policy);

            options.AddPolicy(AbleToAddNewEmployeePolicy.Name, AbleToAddNewEmployeePolicy.Policy);
        }
    }
}
