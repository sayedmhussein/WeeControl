using System;
using Microsoft.AspNetCore.Authorization;

namespace MySystem.Web.Api.Security.Requirement
{
    public class MaximumPeriodRequirement : IAuthorizationRequirement
    {
        public TimeSpan Period { get; }

        public MaximumPeriodRequirement(TimeSpan maximumPeriod)
        {
            Period = maximumPeriod;
        }
    }
}
