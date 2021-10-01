using Microsoft.Extensions.DependencyInjection;

namespace WeeControl.SharedKernel
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddUserSecurityService(this IServiceCollection services)
        {
            return services;
        }
    }
}