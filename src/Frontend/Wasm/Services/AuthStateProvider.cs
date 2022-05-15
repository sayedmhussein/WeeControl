using System;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using WeeControl.Common.SharedKernel.Essential;
using WeeControl.Common.SharedKernel.Interfaces;
using WeeControl.Frontend.FunctionalService.Enums;
using WeeControl.Frontend.FunctionalService.EssentialContext;
using WeeControl.Frontend.FunctionalService.Interfaces;
using WeeControl.Frontend.Wasm.Interfaces;

namespace WeeControl.Frontend.Wasm.Services;

public class AuthStateProvider : AuthenticationStateProvider, ISecurityService
{
    private readonly IUserStorage localStorage;
    private readonly IJwtService jwtService;
    private readonly IConfiguration configuration;
    private readonly IUserOperation service;
    private readonly AuthenticationState anonymous;

    public AuthStateProvider(IUserStorage localStorage, IJwtService jwtService, IConfiguration configuration, IUserOperation service)
    {
        this.localStorage = localStorage;
        this.jwtService = jwtService;
        this.configuration = configuration;
        this.service = service;

        //service.TokenChanged += (sender, args) =>
        //{
        //    NotifyUserAuthentication(args);
        //};

        var identity = new ClaimsIdentity();
            
        var cp = new ClaimsPrincipal(identity);
        anonymous = new AuthenticationState(cp);
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await localStorage.GetAsync(UserDataEnum.Token);
        if (string.IsNullOrWhiteSpace(token))
        {
            return anonymous;
        }
            
        var cp = GetClaimPrincipal(token);
        return new AuthenticationState(cp);
    }

    public void NotifyUserAuthentication(string token)
    {
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
        
    [Obsolete]
    public async Task NotifyUserAuthenticationAsyc()
    {
        var token = await localStorage.GetAsync(UserDataEnum.Token);
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
        var key = configuration["Jwt:Key"] ?? throw new NullReferenceException("Jwt:Key in IConfiguration can't be null!");
            
        var validationParameters = new TokenValidationParameters()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false,
            ClockSkew = TimeSpan.Zero
        };

        return jwtService.ExtractClaimPrincipal(token);
    }

    public void Update(string token)
    {
        NotifyUserAuthentication(token);
    }
}