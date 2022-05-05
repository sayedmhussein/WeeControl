using Microsoft.Extensions.DependencyInjection;

namespace WeeControl.Common.SharedKernel
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddUserSecurityService(this IServiceCollection services)
        {
            return services;
        }
    }
}