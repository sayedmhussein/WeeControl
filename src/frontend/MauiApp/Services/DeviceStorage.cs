using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using WeeControl.Common.SharedKernel.Interfaces;
using WeeControl.Frontend.AppService.Interfaces.GuiInterfaces;

namespace WeeControl.Frontend.MauiApp.Services;

public class DeviceStorage : IStorage, IDeviceSecurity
{
    private readonly IJwtService jwtService;
    public string CashDirectory => FileSystem.CacheDirectory;
    public string AppDataDirectory => FileSystem.AppDataDirectory;

    public DeviceStorage(IJwtService jwtService)
    {
        this.jwtService = jwtService;
    }
    
    public Task SaveAsync(string key, string value)
    {
        return SecureStorage.Default.SetAsync(key, value);
    }

    public Task<string> GetAsync(string key)
    {
        return SecureStorage.Default.GetAsync(key);
    }

    public Task ClearAsync()
    {
        SecureStorage.Default.RemoveAll();
        return Task.CompletedTask;
    }

    public async Task<bool> IsAuthenticatedAsync()
    {
        var claims = await GetClaimsAsync();

        return claims.Count() > 1;
    }

    public Task UpdateTokenAsync(string token)
    {
        return SaveAsync("token", token);
    }

    public Task<string> GetTokenAsync()
    {
        return GetAsync("token");
    }

    public Task DeleteTokenAsync()
    {
        return SaveAsync("token", "");
    }

    public async Task<IEnumerable<Claim>> GetClaimsAsync()
    {
        var token = await GetTokenAsync();
        var cp = jwtService.GetClaimPrincipal(token, new TokenValidationParameters()
        {
            ValidateLifetime = true,
            ValidateAudience = false,
            ValidateIssuer = false
        });

        return cp.Claims;
    }
}