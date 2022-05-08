using System;
using Microsoft.AspNetCore.Http;
using Xunit;
using WeeControl.Backend.WebApi.Middlewares;
using System.Threading.Tasks;
using System.Net;

namespace WeeControl.Backend.WebApi.Test.Middlewares
{
    public class CustomExceptionHandlerMiddlewareTests
    {
        [Fact]
        public async void WhenContextNotThrowException_TaskInvoked()
        {
            var httpContext = new DefaultHttpContext();
            httpContext.Response.StatusCode = 200;
            //
            var requestDelegate = new RequestDelegate((innerContext) => Task.FromResult(0));
            var middleware = new CustomExceptionHandlerMiddleware(requestDelegate);

            await middleware.Invoke(httpContext);

            Assert.Equal(HttpStatusCode.OK, (HttpStatusCode)httpContext.Response.StatusCode);
        }

        [Fact]
        public async void WhenContextThrowException_TaskInvoked()
        {
            var httpContext = new DefaultHttpContext();
            httpContext.Response.StatusCode = 200;
            //
            var requestDelegate = new RequestDelegate((innerContext) => throw new Exception());
            var middleware = new CustomExceptionHandlerMiddleware(requestDelegate);

            await middleware.Invoke(httpContext);

            Assert.Equal(HttpStatusCode.InternalServerError, (HttpStatusCode)httpContext.Response.StatusCode);
        }
    }
}
