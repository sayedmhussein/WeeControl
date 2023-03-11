using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using WeeControl.Core.Application.Exceptions;
using WeeControl.Core.SharedKernel.Exceptions;

namespace WeeControl.Host.WebApi.Middlewares;

public class CustomExceptionHandlerMiddleware
{
    private readonly RequestDelegate next;

    public CustomExceptionHandlerMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        HttpStatusCode code;
        var result = string.Empty;

        switch (exception)
        {
            case EntityModelValidationException entityException:
                context.Response.StatusCode = (int) HttpStatusCode.BadRequest;
                return context.Response.WriteAsync(JsonConvert.SerializeObject(entityException.Failures));
            case BadRequestException badRequestException:
                code = HttpStatusCode.BadRequest;
                result = badRequestException.Message;
                break;
            case EntityDomainValidationException domainOutOfRanceException:
                code = HttpStatusCode.BadRequest;
                result = domainOutOfRanceException.Message;
                break;
            case NotFoundException:
                code = HttpStatusCode.NotFound;
                break;
            case NotAllowedException:
                code = HttpStatusCode.Forbidden;
                break;
            case ConflictFailureException:
                code = HttpStatusCode.Conflict;
                break;
            default:
                code = HttpStatusCode.InternalServerError;
#if DEBUG
                result = exception.StackTrace;
#else
                result = exception.Message;
#endif

                break;
        }

        if (result == string.Empty) result = "Check statues code";

        context.Response.StatusCode = (int) code;
        return context.Response.WriteAsync(GetSingleLineErrorSerialized(result));
    }

    private static string GetSingleLineErrorSerialized(string errorMessage)
    {
        return "{\"Error\": \"" + errorMessage + "\"}";
    }
}

public static class CustomExceptionHandlerMiddlewareExtensions
{
    public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<CustomExceptionHandlerMiddleware>();
    }
}