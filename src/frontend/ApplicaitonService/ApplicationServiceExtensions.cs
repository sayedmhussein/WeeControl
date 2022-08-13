using Microsoft.Extensions.DependencyInjection;
using WeeControl.Frontend.ApplicationService.Anonymous.Interfaces;
using WeeControl.Frontend.ApplicationService.Anonymous.ViewModels;
using WeeControl.Frontend.ApplicationService.Common;
using WeeControl.Frontend.ApplicationService.Customer.ViewModels;
using WeeControl.Frontend.ApplicationService.Interfaces;
using WeeControl.Frontend.ApplicationService.Services;

namespace WeeControl.Frontend.ApplicationService;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddViewModels(this IServiceCollection services)
    {
        services.AddSingleton<IPersistedLists, PersistedListService>();
        services.AddTransient<IServerOperation, ServerOperationService>();
        
        services.AddTransient<IAuthorizationViewModel, AuthorizationViewModel>();
        services.AddTransient<IPublicViewModel, PublicViewModel>();
        
        
        
        services.AddTransient<IUserHomeViewModel, UserHomeViewModel>();
        services.AddTransient<TerritoryViewModel>();

        #region AuthorizationContext
        services.AddTransient<PasswordChangeViewModel>();
        #endregion

        return services;
    }
}