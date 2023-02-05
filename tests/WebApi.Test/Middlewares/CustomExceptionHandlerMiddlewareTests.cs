using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Threading.Tasks;
using WeeControl.Core.Application.Exceptions;
using WeeControl.Host.WebApi.Middlewares;
using Xunit;

namespace WeeControl.ApiApp.WebApi.Test.Middlewares;

public class CustomExceptionHandlerMiddlewareTests
{
    [Fact]
    public async void WhenContextNotThrowException_TaskInvokedAndResponseIsSame()
    {
        var httpContext = new DefaultHttpContext
        {
            Response =
            {
                StatusCode = 200
            }
        };
        var requestDelegate = new RequestDelegate((innerContext) => Task.FromResult(0));
        var middleware = new CustomExceptionHandlerMiddleware(requestDelegate);

        await middleware.Invoke(httpContext);

        Assert.Equal(HttpStatusCode.OK, (HttpStatusCode)httpContext.Response.StatusCode);
    }

    [Fact]
    public async void WhenContextThrowException_TaskInvokedAndResponseIsError()
    {
        var httpContext = new DefaultHttpContext
        {
            Response =
            {
                StatusCode = 200
            }
        };
        var requestDelegate = new RequestDelegate((innerContext) => throw new Exception());
        var middleware = new CustomExceptionHandlerMiddleware(requestDelegate);

        await middleware.Invoke(httpContext);

        Assert.Equal(HttpStatusCode.InternalServerError, (HttpStatusCode)httpContext.Response.StatusCode);
    }

    [Fact]
    public async void WhenContextThrowNotFoundException_TaskInvokedAndResponseIsNotFound()
    {
        var httpContext = new DefaultHttpContext
        {
            Response =
            {
                StatusCode = 200
            }
        };
        var requestDelegate = new RequestDelegate((innerContext) => throw new NotFoundException("Unit testing"));
        var middleware = new CustomExceptionHandlerMiddleware(requestDelegate);

        await middleware.Invoke(httpContext);

        Assert.Equal(HttpStatusCode.NotFound, (HttpStatusCode)httpContext.Response.StatusCode);
    }
}