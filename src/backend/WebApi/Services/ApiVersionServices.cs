using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.DependencyInjection;

namespace WeeControl.ApiApp.WebApi.Services;

public static class ApiVersionServices
{
    public static IServiceCollection AddApiVersionService(this IServiceCollection services)
    {
        services.AddApiVersioning(ConfigureApiVersioning);
        return services;
    }

    private static void ConfigureApiVersioning(ApiVersioningOptions options)
    {
        options.DefaultApiVersion = new ApiVersion(1, 0);
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.ApiVersionReader = new MediaTypeApiVersionReader();
        options.ReportApiVersions = true;
    }
}