using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MySystem.Shared.Library.Definition;

namespace Sayed.MySystem.Api.Middleware
{
    public class UserInfoMiddleware
    {
        private readonly RequestDelegate next;

        public UserInfoMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        //Below you can inject scoped
        public async Task Invoke(HttpContext httpContext)
        {
            var sessionid = httpContext.User.Claims.FirstOrDefault(c => c.Type == UserClaim.Session)?.Value;
            if (Guid.TryParse(sessionid, out Guid _))
                httpContext.Items.Add(UserClaim.Session, sessionid);


            await next(httpContext);
        }
    }
}
