using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

//[assembly: InternalsVisibleTo("MySystem.Api.UnitTest")]
namespace MySystem.Web.Api.Security.TokenRefreshment
{
    public class TokenRefreshmentHandler : AuthorizationHandler<TokenRefreshmentRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                       TokenRefreshmentRequirement requirement)
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
