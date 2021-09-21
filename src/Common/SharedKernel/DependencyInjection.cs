using Microsoft.Extensions.DependencyInjection;
using WeeControl.Common.SharedKernel.EntityGroups.Territory.Attributes;
using WeeControl.Common.SharedKernel.EntityGroups.Territory.Interfaces;

namespace WeeControl.Common.SharedKernel
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddUserSecurityService(this IServiceCollection services)
        {
            services.AddSingleton<ITerritoryAttribute, TerritoryAppSetting>();
            
            
            return services;
        }
    }
}