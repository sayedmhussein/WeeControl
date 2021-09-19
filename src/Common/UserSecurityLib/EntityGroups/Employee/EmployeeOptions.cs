using Microsoft.AspNetCore.Authorization;
using WeeControl.UserSecurityLib.EntityGroups.Employee.Policies;

namespace WeeControl.UserSecurityLib.EntityGroups.Employee
{
    public static class EmployeeOptions
    {
        public static void Configure(AuthorizationOptions options)
        {
            options.AddPolicy(
                CustomAuthorizationPolicy.Employee.CanAlterEmployee,
                new CanAlterEmployeePolicy().GetPolicy());
        }
    }
}