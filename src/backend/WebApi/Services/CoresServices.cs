using Microsoft.Extensions.DependencyInjection;

namespace WeeControl.Host.WebApi.Services;

public static class CoresServices
{
    public static IServiceCollection AddCoresService(this IServiceCollection services)
    {
        services.AddCors(c => c.AddPolicy("AllowAny", builder =>
        {
            builder.AllowAnyHeader();
            builder.AllowAnyMethod();
            builder.AllowAnyOrigin();
        }));

        return services;
    }
}