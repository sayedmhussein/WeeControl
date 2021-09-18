using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using WeeControl.Frontend.CommonLib.Interfaces;
using WeeControl.SharedKernel.Interfaces;

namespace WeeControl.Frontend.Wasm.Services
{
    public class AuthStateProvider : AuthenticationStateProvider, IAuthenticationBase
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
            var cp = jwtService.GetClaims(token);
            var authState = Task.FromResult(new AuthenticationState(cp));
            
            NotifyAuthenticationStateChanged(authState);
        }
    }
}