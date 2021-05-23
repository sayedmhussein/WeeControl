using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using MySystem.Web.Api.Security.Requirement;
using MySystem.Web.EntityFramework;

[assembly: InternalsVisibleTo("MySystem.Api.UnitTest")]
namespace MySystem.Web.Api.Security.Handler
{
    public class SessionAndUserValidHandler : AuthorizationHandler<SessionAndUserValidRequirement>
    {
        private readonly DataContext dbContext;

        public SessionAndUserValidHandler(DataContext dbContext)
        {
            this.dbContext = dbContext;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                       SessionAndUserValidRequirement requirement)
        {
            var sessionid = context.User.Claims.FirstOrDefault(x => x.Type == "sss")?.Value;
            if (sessionid != null)
            {
                
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
