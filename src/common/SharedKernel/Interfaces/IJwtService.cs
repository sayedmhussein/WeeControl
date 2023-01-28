using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace WeeControl.Core.SharedKernel.Interfaces;

public interface IJwtService
{
    string GenerateToken(SecurityTokenDescriptor descriptor);

    ClaimsPrincipal GetClaimPrincipal(string token, TokenValidationParameters? parameters);
}