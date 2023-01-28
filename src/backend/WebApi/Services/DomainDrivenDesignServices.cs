using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using WeeControl.ApiApp.Infrastructure;
using WeeControl.ApiApp.Persistence;
using WeeControl.Core.Application;

namespace WeeControl.ApiApp.WebApi.Services;

public static class DomainDrivenDesignServices
{
    public static IServiceCollection AddDomainDrivenDesignService(this IServiceCollection services, IConfiguration Configuration)
    {
        services.AddApplication();
        services.AddInfrastructure(Configuration);

        _ = Configuration["UseInMemoryDb"] == false.ToString() ?
            services.AddPersistenceAsPostgres(Configuration, Assembly.GetExecutingAssembly().GetName().Name) :
            services.AddPersistenceAsInMemory();


        return services;
    }
}