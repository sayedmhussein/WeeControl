using Microsoft.AspNetCore.Authorization;
using WeeControl.Server.WebApi.Security.Policies.Employee;
using WeeControl.Server.WebApi.Security.Policies.Territory;

namespace WeeControl.Server.WebApi.StartupOptions
{
    public class AuthorOptions
    {
        public static void ConfigureAuthOptions(AuthorizationOptions options)
        {
            options.AddPolicy(CanGetTerritoryPolicy.Name, CanGetTerritoryPolicy.Policy);
            options.AddPolicy(CanAddEditTerritoryPolicy.Name, CanAddEditTerritoryPolicy.Policy);
            options.AddPolicy(CanDeleteTerritoryPolicy.Name, CanDeleteTerritoryPolicy.Policy);

            options.AddPolicy(AbleToAddNewEmployeePolicy.Name, AbleToAddNewEmployeePolicy.Policy);

            options.AddPolicy(AbleToRefreshTokenPolicy.Name, AbleToRefreshTokenPolicy.Policy);
        }
    }
}
