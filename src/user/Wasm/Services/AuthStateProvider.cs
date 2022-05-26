using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using WeeControl.SharedKernel.Essential.DataTransferObjects;
using WeeControl.SharedKernel.Interfaces;
using WeeControl.User.UserServiceCore.Interfaces;

namespace WeeControl.User.Wasm.Services;

public class AuthStateProvider : AuthenticationStateProvider, IDeviceSecurity
{
    private readonly IDeviceStorage localStorage;
    private readonly IJwtService jwtService;
    private readonly IConfiguration configuration;
    private readonly AuthenticationState anonymous;
    private readonly string tokenKeyName;

    public AuthStateProvider(IDeviceStorage localStorage, IJwtService jwtService, IConfiguration configuration)
    {
        this.localStorage = localStorage;
        this.jwtService = jwtService;
        this.configuration = configuration;
        
        var identity = new ClaimsIdentity();
        var cp = new ClaimsPrincipal(identity);
        anonymous = new AuthenticationState(cp);

        tokenKeyName = nameof(TokenDtoV1.Token);
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await localStorage.GetAsync(tokenKeyName);
        if (string.IsNullOrWhiteSpace(token))
        {
            return anonymous;
        }
            
        var cp = GetClaimPrincipal(token);
        return new AuthenticationState(cp);
    }

    private void NotifyUserAuthentication(string? token)
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
    
    public async Task<bool> IsAuthenticatedAsync()
    {
        var token = await localStorage.GetAsync(tokenKeyName);
        return !string.IsNullOrWhiteSpace(token);
    }

    public Task UpdateTokenAsync(string token)
    {
        localStorage.SaveAsync(tokenKeyName, token);
        NotifyUserAuthentication(token);
        return Task.CompletedTask;
    }

    public Task DeleteTokenAsync()
    {
        return localStorage.SaveAsync(tokenKeyName, string.Empty);
    }

    public async Task<IEnumerable<Claim>> GetClaimsAsync()
    {
        try
        {
            var token = await localStorage.GetAsync(tokenKeyName);
            var cp = GetClaimPrincipal(token);
            return cp.Claims;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new List<Claim>();
        }
    }
}