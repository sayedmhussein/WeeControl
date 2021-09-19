using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using WeeControl.Frontend.CommonLib.Interfaces;

namespace WeeControl.Frontend.Wasm.Services
{
    public class AuthenticationRefreshService : IAuthenticationRefresh
    {
        private readonly AuthStateProvider provider;

        public AuthenticationRefreshService(AuthenticationStateProvider provider)
        {

            this.provider = (AuthStateProvider)provider;

        }
        public async Task AuthenticationRefreshedAsync()
        {
            await provider.NotifyUserAuthentication();
        }
    }
}