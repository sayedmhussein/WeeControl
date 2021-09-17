using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using WeeControl.Frontend.CommonLib.Interfaces;
using WeeControl.SharedKernel.Interfaces;

namespace WeeControl.Frontend.Wasm.Services
{
    public class AuthStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorage localStorage;
        private readonly IJwtService jwtService;

        public AuthStateProvider(ILocalStorage localStorage, IJwtService jwtService)
        {
            this.localStorage = localStorage;
            this.jwtService = jwtService;
        }
        
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await localStorage.GetItem<string>("Token");
            if (string.IsNullOrWhiteSpace(token))
            {
                return default;
            }

            var cp = jwtService.GetClaims(token);
            return new AuthenticationState(cp);
        }

        public void NotifyUserAuthentication(string token)
        {
            var cp = jwtService.GetClaims(token);
            var authState = Task.FromResult(new AuthenticationState(cp));
            
            NotifyAuthenticationStateChanged(authState);
        }

        public void NotifyUserLogout()
        {
            var authState = Task.FromResult((AuthenticationState)default);
            
            NotifyAuthenticationStateChanged(authState);
        }
    }
}