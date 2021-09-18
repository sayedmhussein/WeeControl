using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using WeeControl.Frontend.CommonLib.Interfaces;
using WeeControl.SharedKernel.Interfaces;

namespace WeeControl.Frontend.Wasm.Services
{
    public class AuthenticationRefresh : IAuthenticationRefresh
    {
        private readonly AuthStateProvider provider;

        public AuthenticationRefresh(AuthStateProvider provider)
        {
            this.provider = provider;
        }
        
        public async Task AuthenticationRefreshedAsync()
        {
            await provider.NotifyUserAuthentication();
        }
    }
}