using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using MySystem.Web.Api.Security.Requirement;

[assembly: InternalsVisibleTo("MySystem.Api.UnitTest")]
namespace MySystem.Web.Api.Security.Handler
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
                var savedDate = DateTime.UnixEpoch.AddSeconds(__savedDate);

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
