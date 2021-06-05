using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace MySystem.MySystem.Api.Middleware
{
    public class ExternalMessagingMiddleware
    {
        private readonly RequestDelegate next;

        public ExternalMessagingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            await next(context);
        }
    }

    public static class ExternalMessagingMiddlewareExtensions
    {
        public static IApplicationBuilder UseExternalMessagingHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExternalMessagingMiddleware>();
        }
    }
}
