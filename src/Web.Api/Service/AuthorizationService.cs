using System;
using Microsoft.AspNetCore.Authorization;
using MySystem.Web.Api.Security.Policies.Employee;

namespace MySystem.MySystem.Api.Service
{
    public class AuthorizationService
    {
        public static void ConfigureAuthorizationOptions(AuthorizationOptions options)
        {
            options.AddPolicy(AbleToAddNewEmployeePolicy.Name, AbleToAddNewEmployeePolicy.Policy);
        }
    }
}
