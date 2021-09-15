using System;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using WeeControl.Frontend.CommonLib.DataAccess;

namespace WeeControl.Frontend.CommonLib
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRefitService(this IServiceCollection services)
        {
            services.AddRefitClient<ITerritoryData>().ConfigureHttpClient(c => c.BaseAddress = new Uri(""));
            return services;
        }
        
    }

}
