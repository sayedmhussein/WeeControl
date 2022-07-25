using Microsoft.Extensions.DependencyInjection;
using WeeControl.Frontend.ApplicationService.Essential.ViewModels;
using WeeControl.Frontend.ApplicationService.Interfaces;
using WeeControl.Frontend.ApplicationService.Services;

namespace WeeControl.Frontend.ApplicationService;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddViewModels(this IServiceCollection services)
    {
        services.AddSingleton<IPersistedLists, PersistedListService>();
        services.AddTransient<IServerOperation, ServerOperationService>();

        services.AddTransient<HomeViewModel>();
        services.AddTransient<AuthorizationViewModel>();
        services.AddTransient<UserViewModel>();
        services.AddTransient<TerritoryViewModel>();

        #region AuthorizationContext
        services.AddTransient<PasswordResetViewModel>();
        services.AddTransient<PasswordChangeViewModel>();
        #endregion

        return services;
    }
}