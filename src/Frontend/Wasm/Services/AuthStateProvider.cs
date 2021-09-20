using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using WeeControl.Common.UserSecurityLib.Interfaces;
using WeeControl.Frontend.CommonLib.Interfaces;

namespace WeeControl.Frontend.Wasm.Services
{
    public class AuthStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorage localStorage;
        private readonly IJwtService jwtService;
        private readonly AuthenticationState anonymous;

        public AuthStateProvider(ILocalStorage localStorage, IJwtService jwtService)
        {
            this.localStorage = localStorage;
            this.jwtService = jwtService;

            var identity = new ClaimsIdentity();
            
            var cp = new ClaimsPrincipal(identity);
            anonymous = new AuthenticationState(cp);
        }
        
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await localStorage.GetItem<string>("Token");
            if (string.IsNullOrWhiteSpace(token))
            {
                return anonymous;
            }
            
            var cp = jwtService.GetClaims(token);
            return new AuthenticationState(cp);
        }

        public async Task NotifyUserAuthentication()
        {
            var token = await localStorage.GetItem<string>("Token");
            if (string.IsNullOrWhiteSpace(token) == false)
            {
                var cp = jwtService.GetClaims(token);
                var state = new AuthenticationState(cp);
                var authState = Task.FromResult(state);
                NotifyAuthenticationStateChanged(authState);
            }
            else
            {
                NotifyAuthenticationStateChanged(Task.FromResult(anonymous));
            }
        }
    }
}