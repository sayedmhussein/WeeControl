using Microsoft.AspNetCore.Authorization;
using WeeControl.Server.WebApi.Security.Policies.Common;
using WeeControl.Server.WebApi.Security.Policies.Employee;
using WeeControl.Server.WebApi.Security.Policies.Territory;

namespace WeeControl.Server.WebApi.StartupOptions
{
    public class AuthorOptions
    {
        public static void ConfigureAuthOptions(AuthorizationOptions options)
        {
            options.AddPolicy(HasSessionPolicy.Name, HasSessionPolicy.Policy);

            options.AddPolicy(CanAlterTerritories.Name, CanAlterTerritories.Policy);

            options.AddPolicy(AbleToAddNewEmployeePolicy.Name, AbleToAddNewEmployeePolicy.Policy);

            options.AddPolicy(AbleToRefreshTokenPolicy.Name, AbleToRefreshTokenPolicy.Policy);
        }
    }
}
