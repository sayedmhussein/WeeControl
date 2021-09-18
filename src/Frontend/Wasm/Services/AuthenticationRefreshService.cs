using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using WeeControl.Frontend.CommonLib.Interfaces;
using WeeControl.Frontend.CommonLib.Services;

namespace WeeControl.Frontend.Wasm.Services
{
    public class AuthenticationRefreshService : IAuthenticationRefresh
    {
        private readonly AuthStateProvider provider;

        public AuthenticationRefreshService(IServiceProvider provider)
        {
            
            var bla = provider.GetService(typeof(AuthenticationState));
            
        }
        public async Task AuthenticationRefreshedAsync()
        {
            await provider.NotifyUserAuthentication();
        }
    }
}