using System;
using CommonLib.DataAccess;
using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace CommonLib
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
