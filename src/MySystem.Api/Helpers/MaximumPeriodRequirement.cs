using System;
using Microsoft.AspNetCore.Authorization;

namespace MySystem.Api.Helpers
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
