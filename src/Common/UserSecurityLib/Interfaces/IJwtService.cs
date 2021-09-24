using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace WeeControl.Common.UserSecurityLib.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(SecurityTokenDescriptor descriptor);
        
        ClaimsPrincipal ExtractClaimPrincipal(TokenValidationParameters parameters, string token);
    }
}