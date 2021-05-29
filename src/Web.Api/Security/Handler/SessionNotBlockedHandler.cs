using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using MySystem.SharedKernel.Definition;
using MySystem.Web.Api.Security.Requirement;

//[assembly: InternalsVisibleTo("MySystem.Api.UnitTest")]
namespace MySystem.Web.Api.Security.Handler
{
    public class SessionNotBlockedHandler : AuthorizationHandler<SessionNotBlockedRequirement>
    {

        public SessionNotBlockedHandler()
        {
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                       SessionNotBlockedRequirement requirement)
        {
            var sessionid = context.User.Claims.FirstOrDefault(x => x.Type == UserClaim.Session)?.Value;
            if (sessionid != null)
            {
                //if (await employeeService.GetUserIdAsync(Guid.Parse(sessionid)) == null)
                //{
                //    context.Fail();
                //}

                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
