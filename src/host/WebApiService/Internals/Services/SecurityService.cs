using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using WeeControl.Core.SharedKernel.Interfaces;
using WeeControl.Host.WebApiService.DeviceInterfaces;
using WeeControl.Host.WebApiService.Internals.Interfaces;

namespace WeeControl.Host.WebApiService.Internals.Services;

internal class SecurityService : IDeviceSecurity
{
    private readonly IStorage storage;
    private readonly IJwtService jwtService;

    public SecurityService(IStorage storage, IJwtService jwtService)
    {
        this.storage = storage;
        this.jwtService = jwtService;
    }
    
    public async Task<bool> IsAuthenticated()
    {
        return !string.IsNullOrWhiteSpace(await storage.GetKeyValue(IDeviceSecurity.TokenKeyName));
    }

    public Task UpdateToken(string token)
    {
        TokenChanged?.Invoke(this, token);
        return storage.SaveKeyValue(IDeviceSecurity.TokenKeyName, token);
    }

    public Task<string?> GetToken()
    {
        return storage.GetKeyValue(IDeviceSecurity.TokenKeyName);
    }

    public Task DeleteToken()
    {
        return storage.SaveKeyValue(IDeviceSecurity.TokenKeyName, string.Empty);
    }

    public async Task<ClaimsPrincipal> GetClaimsPrincipal()
    {
        var token = await GetToken();
        if (string.IsNullOrEmpty(token))
            return new ClaimsPrincipal();

        const string key = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c";

        var validationParameters = new TokenValidationParameters()
        {
            RequireSignedTokens = false,
            RequireAudience = false,
            RequireExpirationTime = false,
            LogValidationExceptions = true,
            ValidateIssuerSigningKey = false,
            //IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
            SignatureValidator = delegate (string token, TokenValidationParameters parameters)
            {
                var jwt = new JwtSecurityToken(token);

                return jwt;
            },
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false,
            ClockSkew = TimeSpan.Zero
        };

        validationParameters.RequireSignedTokens = false;

        var cp = jwtService.GetClaimPrincipal(token, validationParameters);
        return cp;
    }

    public event EventHandler<string>? TokenChanged;
}