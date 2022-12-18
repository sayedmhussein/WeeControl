using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using WeeControl.Common.SharedKernel.Interfaces;
using WeeControl.Frontend.AppService.DeviceInterfaces;
using WeeControl.Frontend.AppService.Internals.Interfaces;

namespace WeeControl.Frontend.AppService.Internals.Services;

internal class SecurityService : IDeviceSecurity
{
    private const string TokenKeyName = nameof(TokenKeyName);
    private readonly IStorage storage;
    private readonly IJwtService jwtService;

    public SecurityService(IStorage storage, IJwtService jwtService)
    {
        this.storage = storage;
        this.jwtService = jwtService;
    }
    
    public async Task<bool> IsAuthenticatedAsync()
    {
        return !string.IsNullOrWhiteSpace(await storage.GetAsync(TokenKeyName));
    }

    public Task UpdateTokenAsync(string token)
    {
        return storage.SaveAsync(TokenKeyName, token);
    }

    public Task<string?> GetTokenAsync()
    {
        return storage.GetAsync(TokenKeyName);
    }

    public Task DeleteTokenAsync()
    {
        return storage.SaveAsync(TokenKeyName, string.Empty);
    }

    public async Task<ClaimsPrincipal> GetClaimsPrincipal()
    {
        var token = await GetTokenAsync();
        if (string.IsNullOrEmpty(token))
            return new ClaimsPrincipal();
        
        var validationParameters = new TokenValidationParameters()
                {
                    RequireSignedTokens = false,
                    RequireAudience = false, RequireExpirationTime = false, LogValidationExceptions = true,
                    ValidateIssuerSigningKey = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(new string('a', 30))),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false,
                    ClockSkew = TimeSpan.Zero
                };
        
        var cp = jwtService.GetClaimPrincipal( token, validationParameters);
        return cp;
    }
}