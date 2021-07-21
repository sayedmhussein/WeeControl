﻿using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using WeeControl.Server.WebApi.Security.TokenRefreshment.CustomHandlers;
using WeeControl.SharedKernel.BasicSchemas.Employee;
using WeeControl.SharedKernel.BasicSchemas.Employee.Enums;

namespace WeeControl.Server.WebApi.Security.Policies
{
    public class BasicPolicies
    {
        public const string HasActiveSession = "HasSessionPolicy";
        //
        public const string CanAlterTerritories = "CanAlterTerritories";
        //
        public const string CanAddNewEmployee = "CanAddNewEmployee";
        public const string CanEditEmployeeDetails = "CanEditEmployeeDetails";

        private readonly IEmployeeLists employeeLists;

        public BasicPolicies(IEmployeeLists employeeLists)
        {
            this.employeeLists = employeeLists;
        }

        public void BuildOptions(AuthorizationOptions options)
        {
            options.AddPolicy(HasActiveSession, GetPolicy(HasActiveSession));
            //
            options.AddPolicy(CanAlterTerritories, GetPolicy(CanAlterTerritories));
            //
            options.AddPolicy(CanAddNewEmployee, GetPolicy(CanAddNewEmployee));
            options.AddPolicy(CanEditEmployeeDetails, GetPolicy(CanEditEmployeeDetails));
        }

        private AuthorizationPolicy GetPolicy(string policy)
        {
            var p = new AuthorizationPolicyBuilder();
            p.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
            p.RequireClaim(employeeLists.GetClaimType(ClaimTypeEnum.Session));

            switch (policy)
            {
                case CanAlterTerritories:
                    p.RequireClaim(employeeLists.GetClaimType(ClaimTypeEnum.Session));
                    p.Requirements.Add(new TokenRefreshmentRequirement(TimeSpan.FromMinutes(5)));
                    break;
                case CanAddNewEmployee:
                    break;
                case CanEditEmployeeDetails:
                    break;
                default:
                    break;
            }

            return p.Build();
        }
    }
}