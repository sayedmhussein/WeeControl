using System;
using Microsoft.AspNetCore.Authorization;

namespace Sayed.MySystem.Api.Helpers
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
