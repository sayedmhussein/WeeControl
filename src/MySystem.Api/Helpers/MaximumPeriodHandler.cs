using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Sayed.MySystem.Api.Helpers
{
    public class MaximumPeriodHandler : AuthorizationHandler<MaximumPeriodRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                       MaximumPeriodRequirement requirement)
        {
            var _savedDate = context.User.Claims.FirstOrDefault(x => x.Type == "iat")?.Value;
            if (_savedDate != null)
            {
                var __savedDate = Convert.ToDouble(_savedDate);
                var savedDate = new DateTime(1970, 1, 1).AddSeconds(__savedDate);

                var diff = DateTime.UtcNow - savedDate;
                if (diff < requirement.Period)
                {
                    context.Succeed(requirement);
                }
            }

            return Task.CompletedTask;
        }
    }
}
