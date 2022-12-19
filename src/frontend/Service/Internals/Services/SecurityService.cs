using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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
        return !string.IsNullOrWhiteSpace(await storage.GetKeyValue(TokenKeyName));
    }

    public Task UpdateTokenAsync(string token)
    {
        return storage.SaveKeyValue(TokenKeyName, token);
    }

    public Task<string?> GetTokenAsync()
    {
        return storage.GetKeyValue(TokenKeyName);
    }

    public Task DeleteTokenAsync()
    {
        return storage.SaveKeyValue(TokenKeyName, string.Empty);
    }

    public async Task<ClaimsPrincipal> GetClaimsPrincipal()
    {
        var token = await GetTokenAsync();
        if (string.IsNullOrEmpty(token))
            return new ClaimsPrincipal();

        const string key = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c";
        
        var validationParameters = new TokenValidationParameters()
                {
                    RequireSignedTokens = false,
                    RequireAudience = false, RequireExpirationTime = false, LogValidationExceptions = true,
                    ValidateIssuerSigningKey = false,
                    //IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
                    SignatureValidator = delegate(string token, TokenValidationParameters parameters)
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
        
        var cp = jwtService.GetClaimPrincipal( token, validationParameters);
        return cp;
    }
}