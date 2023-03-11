using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace WeeControl.Core.SharedKernel.Interfaces;

public interface IJwtService
{
    string GenerateToken(SecurityTokenDescriptor descriptor);

    ClaimsPrincipal GetClaimPrincipal(string token, TokenValidationParameters? parameters);
}