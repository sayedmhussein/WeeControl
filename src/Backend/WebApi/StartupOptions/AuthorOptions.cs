using Microsoft.AspNetCore.Authorization;
using WeeControl.Backend.WebApi.Security.Policies;
using WeeControl.SharedKernel.Aggregates.Employee;

namespace WeeControl.Backend.WebApi.StartupOptions
{
    public class AuthorOptions
    {
        public static void ConfigureAuthOptions(AuthorizationOptions options)
        {
            new BasicPolicies(new EmployeeLists()).BuildOptions(options);
        }
    }
}
