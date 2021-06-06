using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using MySystem.SharedKernel.Enumerators;
using MySystem.SharedKernel.Interfaces;
using MySystem.SharedKernel.Services;
using MySystem.Web.Api.Security.TokenRefreshment;

namespace MySystem.MySystem.Api.Security.Policies
{
    public static class TerritoryPolicies
    {
        public static ImmutableDictionary<PolicyName, AuthorizationPolicy> Policies = GetPoliciesDict().ToImmutableDictionary();

        public static void ConfigureAuthorizationOptions(AuthorizationOptions options)
        {
            options.AddPolicy(nameof(TerritoryPolicies.PolicyName.CanGetPolicy), TerritoryPolicies.Policies[TerritoryPolicies.PolicyName.CanGetPolicy]);
        }

        public enum PolicyName
        {
            CanGetPolicy,
            CanAddPolicy,
            CanEditPolicy,
            CanDeletePolicy
        };

        private static Dictionary<PolicyName, AuthorizationPolicy> GetPoliciesDict()
        {
            var dict = new Dictionary<PolicyName, AuthorizationPolicy>()
            {
                { PolicyName.CanGetPolicy, CanGetPolicy() }
            };

            return dict;
        }

        private static IValuesService values = new ValueService();

        private static AuthorizationPolicy CanGetPolicy()
        {
            var p = new AuthorizationPolicyBuilder();
            p.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
            p.RequireClaim(values.ClaimType[ClaimTypeEnum.Session]);
            p.Requirements.Add(new TokenRefreshmentRequirement(TimeSpan.FromMinutes(100)));

            return p.Build();
        }
    }
}
