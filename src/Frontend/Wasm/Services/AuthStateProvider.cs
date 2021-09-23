using System;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using WeeControl.Common.UserSecurityLib.Interfaces;
using WeeControl.Frontend.CommonLib.Interfaces;

namespace WeeControl.Frontend.Wasm.Services
{
    public class AuthStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorage localStorage;
        private readonly IJwtService jwtService;
        private readonly IConfiguration configuration;
        private readonly AuthenticationState anonymous;

        public AuthStateProvider(ILocalStorage localStorage, IJwtService jwtService, IConfiguration configuration)
        {
            this.localStorage = localStorage;
            this.jwtService = jwtService;
            this.configuration = configuration;

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
            
            var cp = GetClaimPrincipal(token);
            return new AuthenticationState(cp);
        }

        public async Task NotifyUserAuthentication()
        {
            var token = await localStorage.GetItem<string>("Token");
            if (string.IsNullOrWhiteSpace(token) == false)
            {
                var cp = GetClaimPrincipal(token);
                var state = new AuthenticationState(cp);
                var authState = Task.FromResult(state);
                NotifyAuthenticationStateChanged(authState);
            }
            else
            {
                NotifyAuthenticationStateChanged(Task.FromResult(anonymous));
            }
        }

        private ClaimsPrincipal GetClaimPrincipal(string token)
        {
            var validationParameters = new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["Jwt:Key"])),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
            
            return jwtService.ExtractClaimPrincipal(validationParameters, token);
        }
    }
}