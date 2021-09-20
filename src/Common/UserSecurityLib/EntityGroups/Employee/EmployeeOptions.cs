using Microsoft.AspNetCore.Authorization;
using WeeControl.Common.UserSecurityLib.EntityGroups.Employee.Policies;

namespace WeeControl.Common.UserSecurityLib.EntityGroups.Employee
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