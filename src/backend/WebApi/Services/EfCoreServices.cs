using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WeeControl.ApiApp.Infrastructure;
using WeeControl.ApiApp.Persistence;
using WeeControl.Core.Application;

namespace WeeControl.Host.WebApi.Services;

public static class EfCoreServices
{
    public static IServiceCollection AddDomainDrivenDesignService(this IServiceCollection services, IConfiguration configuration)
    {
        
        _ = configuration["UseInMemoryDb"] == false.ToString() ?
            services.AddPersistence(configuration.GetConnectionString("MySqlDbProvider"), Assembly.GetExecutingAssembly().GetName().Name) :
            //services.AddPersistenceAsPostgres("MyDB") :
            services.AddPersistenceAsInMemory();
        
        
        services.AddApplication();
        services.AddInfrastructure(configuration);

        


        return services;
    }
}